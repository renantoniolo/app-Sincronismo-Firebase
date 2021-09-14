using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using appSync.Model;
using Firebase.Database;
using Firebase.Database.Query;

namespace appSync.Util
{
    public class FireBaseHelper
    {
        #region Variaveis locais

        private FirebaseClient firebase;
        private string child = "User";

        #endregion

        public FireBaseHelper()
        {
            try
            {
                firebase = new FirebaseClient("KEY-FIREBASE");
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        #region CRUD Firebase

        public async Task<List<User>> GetUser()
        {
            try
            {
                return (await firebase
                  .Child(child)
                  .OnceAsync<User>()).Select(item => new User
                  {
                      Name = item.Object.Name,
                      Email = item.Object.Email,
                      Age = item.Object.Age,
                      Password = item.Object.Password,
                      Observation = item.Object.Observation,
                  }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public async Task AdddUser(User user)
        {
            try
            {
                if (user is null)
                    return;

                await firebase
                  .Child(child)
                  .PostAsync(new User()
                  {
                      Name = user.Name,
                      Email = user.Email,
                      Password = user.Password,
                      Age = user.Age,
                      Observation = user.Observation
                  });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                if (user is null)
                    return;

                var toUpdatePerson = (await firebase
                  .Child(child)
                  .OnceAsync<User>()).Where(a => a.Object.Email == user.Email).FirstOrDefault();

                await firebase
                  .Child(child)
                  .Child(toUpdatePerson.Key)
                  .PutAsync(new User()
                  {
                      Name = user.Name,
                      Email = user.Email,
                      Password = user.Password,
                      Age = user.Age,
                      Observation = user.Observation
                  });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        public async Task DeleteUser(User user)
        {
            try
            {
                if (user is null)
                    return;

                var toDeletePerson = (await firebase
                  .Child(child)
                  .OnceAsync<User>()).Where(a => a.Object.Email == user.Email).FirstOrDefault();
                await firebase.Child(child).Child(toDeletePerson.Key).DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }

        #endregion
    }
}
