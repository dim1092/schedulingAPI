using AutoFixture;
using SchedulingAPI.Models;

namespace SchedulinAPI;

public static class Seeder
{
    public static void Seed(this ScheduleContext scheduleContext)
    {
        if (!scheduleContext.Users.Any())
        {
            Fixture fixture = new Fixture();
            fixture.Customize<User>(user => user.Without(p => p.Id));
            //--- The next two lines add 100 rows to your database
            List<User> products = fixture.CreateMany<User>(100).ToList();
            scheduleContext.AddRange(products);
            scheduleContext.SaveChanges();
        }
    }
}