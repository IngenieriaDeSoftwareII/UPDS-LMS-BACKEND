using Data.Context;
using Data.Enums;
using Data.Models.Reports.Queries;
using Data.Models.Reports.Results;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class ReportsRepository(AppDbContext db) : IReportsRepository
{
    private static (DateTime From, DateTime To) NormalizeRange(DateTime? from, DateTime? to)
    {
        var defaultTo = DateTime.Today.AddDays(1).AddTicks(-1);
        var defaultFrom = new DateTime(1753, 1, 1);

        var f = from ?? defaultFrom;
        var t = to ?? defaultTo;

        if (from.HasValue && f.TimeOfDay == TimeSpan.Zero)
            f = f.Date;
        if (to.HasValue && t.TimeOfDay == TimeSpan.Zero)
            t = t.Date.AddDays(1).AddTicks(-1);

        if (t < f)
            (f, t) = (t, f);

        return (f, t);
    }

    private static decimal Rate(int numerator, int denominator)
    {
        if (denominator <= 0) return 0m;
        return Math.Round((decimal)numerator / denominator, 4);
    }

    public async Task<AdminCoursesReportResult> GetAdminCoursesAsync(AdminCoursesReportQuery query)
    {
        var (from, to) = NormalizeRange(query.From, query.To);

        var coursesQuery = db.Courses.AsNoTracking()
            .Where(c => c.EntityStatus == 1 && c.DeletedAt == null);

        if (query.CategoryId.HasValue)
            coursesQuery = coursesQuery.Where(c => c.CategoriaId == query.CategoryId.Value);

        if (query.TeacherId.HasValue)
            coursesQuery = coursesQuery.Where(c => c.DocenteId == query.TeacherId.Value);

        if (query.Published.HasValue)
            coursesQuery = coursesQuery.Where(c => c.Publicado == query.Published.Value);

        var courses = await coursesQuery
            .Select(c => new
            {
                c.Id,
                c.Titulo,
                c.Publicado,
                c.DocenteId,
                TeacherName = c.Docente != null && c.Docente.Usuario != null && c.Docente.Usuario.Person != null
                    ? (c.Docente.Usuario.Person.FirstName + " " + c.Docente.Usuario.Person.LastName)
                    : null
            })
            .ToListAsync();

        var courseIds = courses.Select(c => c.Id).ToList();

        var inscriptionsQuery = db.Inscriptions.AsNoTracking()
            .Where(i =>
                i.EntityStatus == 1 &&
                i.DeletedAt == null &&
                courseIds.Contains(i.CursoId) &&
                i.CreatedAt >= from &&
                i.CreatedAt <= to);

        var inscriptionsAgg = await inscriptionsQuery
            .GroupBy(i => i.CursoId)
            .Select(g => new
            {
                CourseId = g.Key,
                TotalEnrollments = g.Count(),
                TotalCancellations = g.Count(x => x.Estado == InscriptionEstate.Cancelado),
                TotalCompletions = g.Count(x => x.Estado == InscriptionEstate.Terminado || x.FechaCompletado.HasValue)
            })
            .ToListAsync();

        var byCourseId = inscriptionsAgg.ToDictionary(x => x.CourseId, x => x);

        var rows = courses
            .Select(c =>
            {
                byCourseId.TryGetValue(c.Id, out var m);
                var enrollments = m?.TotalEnrollments ?? 0;
                var completions = m?.TotalCompletions ?? 0;
                return new CourseReportRow
                {
                    CourseId = c.Id,
                    Title = c.Titulo,
                    TeacherId = c.DocenteId,
                    TeacherName = c.TeacherName,
                    TotalEnrollments = enrollments,
                    TotalCancellations = m?.TotalCancellations ?? 0,
                    TotalCompletions = completions,
                    CompletionRate = Rate(completions, enrollments)
                };
            })
            .OrderByDescending(r => r.TotalEnrollments)
            .ToList();

        var enrollmentsByMonth = await inscriptionsQuery
            .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
            .Select(g => new ReportMonthCount
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return new AdminCoursesReportResult
        {
            From = from,
            To = to,
            Courses = rows,
            EnrollmentsByMonth = enrollmentsByMonth
        };
    }

    public async Task<AdminTeachersReportResult> GetAdminTeachersAsync(AdminTeachersReportQuery query)
    {
        var (from, to) = NormalizeRange(query.From, query.To);

        var coursesQuery = db.Courses.AsNoTracking()
            .Where(c => c.EntityStatus == 1 && c.DeletedAt == null && c.DocenteId != null);

        if (query.CategoryId.HasValue)
            coursesQuery = coursesQuery.Where(c => c.CategoriaId == query.CategoryId.Value);

        var courses = await coursesQuery
            .Select(c => new
            {
                c.Id,
                c.Titulo,
                c.Publicado,
                c.CreatedAt,
                TeacherId = c.DocenteId!.Value
            })
            .ToListAsync();

        var teacherIds = courses.Select(c => c.TeacherId).Distinct().ToList();

        var teachers = await db.Teachers.AsNoTracking()
            .Where(t => t.EntityStatus == 1 && teacherIds.Contains(t.Id))
            .Select(t => new
            {
                t.Id,
                TeacherName = t.Usuario.Person != null
                    ? (t.Usuario.Person.FirstName + " " + t.Usuario.Person.LastName)
                    : t.Usuario.UserName
            })
            .ToListAsync();

        var teacherById = teachers.ToDictionary(t => t.Id, t => t);
        var courseById = courses.ToDictionary(c => c.Id, c => c);
        var courseIds = courses.Select(c => c.Id).ToList();

        var inscriptionsQuery = db.Inscriptions.AsNoTracking()
            .Where(i =>
                i.EntityStatus == 1 &&
                i.DeletedAt == null &&
                courseIds.Contains(i.CursoId) &&
                i.CreatedAt >= from &&
                i.CreatedAt <= to);

        var insAggByCourse = await inscriptionsQuery
            .GroupBy(i => i.CursoId)
            .Select(g => new
            {
                CourseId = g.Key,
                TotalEnrollments = g.Count(),
                TotalCancellations = g.Count(x => x.Estado == InscriptionEstate.Cancelado),
                TotalCompletions = g.Count(x => x.Estado == InscriptionEstate.Terminado || x.FechaCompletado.HasValue)
            })
            .ToListAsync();

        var metricsByCourseId = insAggByCourse.ToDictionary(x => x.CourseId, x => x);

        var teacherRows = new List<AdminTeacherReportRow>();
        foreach (var tid in teacherIds)
        {
            var teacherCourses = courses.Where(c => c.TeacherId == tid).ToList();
            var courseRows = teacherCourses.Select(c =>
            {
                metricsByCourseId.TryGetValue(c.Id, out var m);
                var enrollments = m?.TotalEnrollments ?? 0;
                var completions = m?.TotalCompletions ?? 0;
                return new TeacherCourseSummaryRow
                {
                    CourseId = c.Id,
                    Title = c.Titulo,
                    Published = c.Publicado,
                    CreatedAt = c.CreatedAt,
                    TotalEnrollments = enrollments,
                    TotalCancellations = m?.TotalCancellations ?? 0,
                    TotalCompletions = completions,
                    CompletionRate = Rate(completions, enrollments)
                };
            }).OrderByDescending(r => r.TotalEnrollments).ToList();

            var totalEnrollments = courseRows.Sum(r => r.TotalEnrollments);
            var totalCancellations = courseRows.Sum(r => r.TotalCancellations);
            var totalCompletions = courseRows.Sum(r => r.TotalCompletions);

            var teacherCourseIds = teacherCourses.Select(c => c.Id).ToList();
            var monthly = await inscriptionsQuery
                .Where(i => teacherCourseIds.Contains(i.CursoId))
                .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
                .Select(g => new ReportMonthCount
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            teacherRows.Add(new AdminTeacherReportRow
            {
                TeacherId = tid,
                TeacherName = teacherById.TryGetValue(tid, out var t) ? t.TeacherName : $"Docente {tid}",
                TotalCourses = teacherCourses.Count,
                TotalEnrollments = totalEnrollments,
                TotalCancellations = totalCancellations,
                TotalCompletions = totalCompletions,
                CompletionRate = Rate(totalCompletions, totalEnrollments),
                EnrollmentsByMonth = monthly,
                Courses = courseRows
            });
        }

        teacherRows = teacherRows
            .OrderByDescending(t => t.TotalEnrollments)
            .ToList();

        return new AdminTeachersReportResult
        {
            From = from,
            To = to,
            Teachers = teacherRows
        };
    }

    public async Task<TeacherSummaryReportResult> GetTeacherSummaryAsync(string teacherUserId, TeacherSummaryReportQuery query)
    {
        var (from, to) = NormalizeRange(query.From, query.To);

        var teacher = await db.Teachers.AsNoTracking()
            .Where(t => t.EntityStatus == 1 && t.UsuarioId == teacherUserId)
            .Select(t => new
            {
                t.Id,
                TeacherName = t.Usuario.Person != null
                    ? (t.Usuario.Person.FirstName + " " + t.Usuario.Person.LastName)
                    : t.Usuario.UserName
            })
            .FirstOrDefaultAsync();

        if (teacher is null)
            return new TeacherSummaryReportResult { From = from, To = to };

        var courses = await db.Courses.AsNoTracking()
            .Where(c => c.EntityStatus == 1 && c.DeletedAt == null && c.DocenteId == teacher.Id)
            .Select(c => c.Id)
            .ToListAsync();

        var inscriptionsQuery = db.Inscriptions.AsNoTracking()
            .Where(i =>
                i.EntityStatus == 1 &&
                i.DeletedAt == null &&
                courses.Contains(i.CursoId) &&
                i.CreatedAt >= from &&
                i.CreatedAt <= to);

        var totalEnrollments = await inscriptionsQuery.CountAsync();
        var totalCancellations = await inscriptionsQuery.CountAsync(i => i.Estado == InscriptionEstate.Cancelado);
        var totalCompletions = await inscriptionsQuery.CountAsync(i => i.Estado == InscriptionEstate.Terminado || i.FechaCompletado.HasValue);

        var monthly = await inscriptionsQuery
            .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
            .Select(g => new ReportMonthCount
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return new TeacherSummaryReportResult
        {
            From = from,
            To = to,
            TeacherId = teacher.Id,
            TeacherName = teacher.TeacherName,
            TotalCourses = courses.Count,
            TotalEnrollments = totalEnrollments,
            TotalCancellations = totalCancellations,
            TotalCompletions = totalCompletions,
            CompletionRate = Rate(totalCompletions, totalEnrollments),
            EnrollmentsByMonth = monthly
        };
    }

    public async Task<TeacherCoursesReportResult> GetTeacherCoursesAsync(string teacherUserId, TeacherCoursesReportQuery query)
    {
        var (from, to) = NormalizeRange(query.From, query.To);

        var teacher = await db.Teachers.AsNoTracking()
            .Where(t => t.EntityStatus == 1 && t.UsuarioId == teacherUserId)
            .Select(t => new
            {
                t.Id,
                TeacherName = t.Usuario.Person != null
                    ? (t.Usuario.Person.FirstName + " " + t.Usuario.Person.LastName)
                    : t.Usuario.UserName
            })
            .FirstOrDefaultAsync();

        if (teacher is null)
            return new TeacherCoursesReportResult { From = from, To = to };

        var coursesQuery = db.Courses.AsNoTracking()
            .Where(c => c.EntityStatus == 1 && c.DeletedAt == null && c.DocenteId == teacher.Id);

        if (query.Published.HasValue)
            coursesQuery = coursesQuery.Where(c => c.Publicado == query.Published.Value);

        var courses = await coursesQuery
            .Select(c => new { c.Id, c.Titulo, c.Publicado })
            .ToListAsync();

        var courseIds = courses.Select(c => c.Id).ToList();

        var inscriptionsAgg = await db.Inscriptions.AsNoTracking()
            .Where(i =>
                i.EntityStatus == 1 &&
                i.DeletedAt == null &&
                courseIds.Contains(i.CursoId) &&
                i.CreatedAt >= from &&
                i.CreatedAt <= to)
            .GroupBy(i => i.CursoId)
            .Select(g => new
            {
                CourseId = g.Key,
                TotalEnrollments = g.Count(),
                TotalCancellations = g.Count(x => x.Estado == InscriptionEstate.Cancelado),
                TotalCompletions = g.Count(x => x.Estado == InscriptionEstate.Terminado || x.FechaCompletado.HasValue)
            })
            .ToListAsync();

        var metricsByCourseId = inscriptionsAgg.ToDictionary(x => x.CourseId, x => x);

        var rows = courses
            .Select(c =>
            {
                metricsByCourseId.TryGetValue(c.Id, out var m);
                var enrollments = m?.TotalEnrollments ?? 0;
                var completions = m?.TotalCompletions ?? 0;
                return new TeacherCourseSummaryRow
                {
                    CourseId = c.Id,
                    Title = c.Titulo,
                    Published = c.Publicado,
                    TotalEnrollments = enrollments,
                    TotalCancellations = m?.TotalCancellations ?? 0,
                    TotalCompletions = completions,
                    CompletionRate = Rate(completions, enrollments)
                };
            })
            .OrderByDescending(r => r.TotalEnrollments)
            .ToList();

        return new TeacherCoursesReportResult
        {
            From = from,
            To = to,
            TeacherId = teacher.Id,
            TeacherName = teacher.TeacherName,
            Courses = rows
        };
    }

    public async Task<TeacherCourseDetailReportResult> GetTeacherCourseDetailAsync(string teacherUserId, int courseId, TeacherCourseDetailReportQuery query)
    {
        var (from, to) = NormalizeRange(query.From, query.To);

        var teacher = await db.Teachers.AsNoTracking()
            .Where(t => t.EntityStatus == 1 && t.UsuarioId == teacherUserId)
            .Select(t => new
            {
                t.Id,
                TeacherName = t.Usuario.Person != null
                    ? (t.Usuario.Person.FirstName + " " + t.Usuario.Person.LastName)
                    : t.Usuario.UserName
            })
            .FirstOrDefaultAsync();

        if (teacher is null)
            return new TeacherCourseDetailReportResult { From = from, To = to };

        var course = await db.Courses.AsNoTracking()
            .Where(c => c.EntityStatus == 1 && c.DeletedAt == null && c.Id == courseId)
            .Select(c => new { c.Id, c.Titulo, c.DocenteId })
            .FirstOrDefaultAsync();

        if (course is null || course.DocenteId != teacher.Id)
            return new TeacherCourseDetailReportResult { From = from, To = to, TeacherId = teacher.Id, TeacherName = teacher.TeacherName };

        var inscriptionsQuery = db.Inscriptions.AsNoTracking()
            .Where(i =>
                i.EntityStatus == 1 &&
                i.DeletedAt == null &&
                i.CursoId == course.Id &&
                i.CreatedAt >= from &&
                i.CreatedAt <= to);

        var totalEnrollments = await inscriptionsQuery.CountAsync();
        var totalCancellations = await inscriptionsQuery.CountAsync(i => i.Estado == InscriptionEstate.Cancelado);
        var totalCompletions = await inscriptionsQuery.CountAsync(i => i.Estado == InscriptionEstate.Terminado || i.FechaCompletado.HasValue);

        var monthly = await inscriptionsQuery
            .GroupBy(i => new { i.CreatedAt.Year, i.CreatedAt.Month })
            .Select(g => new ReportMonthCount
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return new TeacherCourseDetailReportResult
        {
            From = from,
            To = to,
            TeacherId = teacher.Id,
            TeacherName = teacher.TeacherName,
            CourseId = course.Id,
            CourseTitle = course.Titulo,
            TotalEnrollments = totalEnrollments,
            TotalCancellations = totalCancellations,
            TotalCompletions = totalCompletions,
            CompletionRate = Rate(totalCompletions, totalEnrollments),
            EnrollmentsByMonth = monthly
        };
    }
}

