using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Data.Repos;
using WorkoutsXAM.Models;
using WorkoutsXAM.Utility;

namespace WorkoutsXAM.Data
{
    public class WorkoutDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public Repository<Workout> WorkoutRepo { get; private set; }
        public Repository<Exercise> ExerciseRepo { get; private set; }

        public WorkoutDatabase()
        {
            InitializeAsync().SafeFireAndForget(false, (e) => { Debug.WriteLine("\n\n\n\n db init error: " + e.Message); });
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Workout).Name))
                {
                    await Database.CreateTableAsync(typeof(Workout), CreateFlags.None).ConfigureAwait(false);
                }
                
                if(!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Exercise).Name))
                {
                    await Database.CreateTableAsync(typeof(Exercise), CreateFlags.None).ConfigureAwait(false);
                }

                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(WorkoutExercise).Name))
                {
                    await Database.CreateTableAsync(typeof(WorkoutExercise), CreateFlags.None).ConfigureAwait(false);
                }

                Debug.WriteLine("DB init");
                initialized = true;
            }

            InitializeRepositories();
        }

        void InitializeRepositories()
        {
            WorkoutRepo = new Repository<Workout>(Database);
            ExerciseRepo = new Repository<Exercise>(Database);
        }
    }
}
