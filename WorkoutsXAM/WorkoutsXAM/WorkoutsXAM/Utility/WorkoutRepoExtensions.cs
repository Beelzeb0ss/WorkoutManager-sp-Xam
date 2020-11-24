using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Data.Repos;
using WorkoutsXAM.Models;

namespace WorkoutsXAM.Utility
{
    static class WorkoutRepoExtensions
    {
        public static async Task SetNext(this Repository<Workout> repo, Workout workout)
        {
            var previous = await repo.Get<Workout>(x => x.IsCurrent != 0);
            foreach (Workout w in previous)
            {
                w.IsNext = 0;
                await repo.Update(w);
            }

            workout.IsNext = 1;
            await repo.Update(workout);
        }
    }
}
