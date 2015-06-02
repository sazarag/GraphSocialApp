using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Trinity;
using Trinity.Storage;
using Trinity.Extension;

namespace GraphTwitter
{
    class GraphTwitter
    {
        public unsafe static void Main(string[] args)
        {
            TrinityConfig.CurrentRunningMode = RunningMode.Embedded;
            #region how to use
            Console.WriteLine(Environment.NewLine + "How to use: ");
            Console.WriteLine("posting: <user name> -> <message> ");
            Console.WriteLine("reading: <user name> ");
            Console.WriteLine("following: <user name> follows <another user> ");
            Console.WriteLine("wall: <user name> wall " + Environment.NewLine);
            #endregion

            #region Users
            User Alice = GraphTwitter.CreateUser("Alice");
            User Bob = GraphTwitter.CreateUser("Bob");
            User Charlie = GraphTwitter.CreateUser("Charlie");

            Console.WriteLine(Environment.NewLine + "The user list: ");
            foreach (var cm in Global.LocalStorage.User_Selector())
            {
                Console.WriteLine(cm.name);
            }
            #endregion 
           
            #region ReadKey Secition
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
           {
               string consoleText = Console.ReadLine();
              
               if (!String.IsNullOrEmpty(consoleText))
               {                   
                   if (consoleText.Contains("->")) 
                    {
                        string[] commands = consoleText.Replace("->", ",").Trim().Split(',');
                       Tweet newPost = GraphTwitter.Posting( GetUser(commands[0].Trim()), commands[1].Trim());                     
                    }
                   else if (consoleText.ToLower().Contains("wall"))
                    {
                        string userName = consoleText.Replace("wall", "").Trim();
                        User user = GraphTwitter.GetUser(userName);
                        List<Tweet> tweets = GetWall(user);
                        WriteTweetsToConsole( tweets);                        
                    }
                   else if (consoleText.ToLower().Contains("follows"))
                    {
                        string[] commands = consoleText.Replace("follows", ",").Trim().Split(',');                        
                        GraphTwitter.Following(GraphTwitter.GetUser(commands[0].Trim()), GraphTwitter.GetUser(commands[1].Trim()));

                    }//reading
                    else
                    {
                        string userName = consoleText.Trim();                        
                        User user = GraphTwitter.GetUser(userName);
                       
                        if (user.name != null)
                        {
                            List<Tweet> tweets = GraphTwitter.Reading(user);
                            WriteTweetsToConsole(tweets);
                        }                        
                    }
                }

           }
            #endregion
        }

        /// <summary>
        /// Returns the User where name match with parameter username 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public unsafe static User GetUser(string username)
        {
            return Global.LocalStorage.User_Selector().Where(us => us.name == username).FirstOrDefault();                    
        }

        /// <summary>
        /// Returns the User where Id match with parameter username 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public unsafe static User GetUser(long userId)
        {
            return Global.LocalStorage.User_Selector().Where(us => us.CellID == userId).FirstOrDefault();
        }
        
        /// <summary>
        /// Create User via usernameparameter 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public unsafe static User CreateUser(string userName)
        {
            User newUser = new User(name: userName, tweets: null);
            Global.LocalStorage.SaveUser(newUser);
            //Console.WriteLine("newUserId=" + newUser.CellID);
            FollowList followList = new FollowList(newUser.CellID, new List<long>());
            Global.LocalStorage.SaveFollowList(followList);
            return newUser;            
        }

        /// <summary>
        /// Post the message to users wall
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public unsafe static Tweet Posting(User user, string message)
        {
            Tweet newTweet = new Tweet(user.CellID, DateTime.UtcNow, message);
            Global.LocalStorage.SaveTweet(newTweet);
            return newTweet;
        }

        /// <summary>
        /// Returns the User's messages
        /// </summary>
        /// <param name="user"></param>
        public unsafe static List<Tweet> Reading(User curentUser)
        {
           // var tweetCellIds = Global.LocalStorage.UseUser(user.CellID).tweets;
             return Global.LocalStorage.Tweet_Selector().Where(e => e.user == curentUser.CellID).ToList();            
        }

        /// <summary>
        /// Add a new follow to the user's follow list 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="wanttoFollow"></param>
        /// <returns></returns>
        public unsafe static FollowList Following(User currentUser, User wanttoFollow)
        {
             FollowList currentUsersFollowList = Global.LocalStorage.FollowList_Accessor_Selector().Where(fol => fol.user == currentUser.CellID).FirstOrDefault();

            if( !currentUsersFollowList.follows.Contains(wanttoFollow.CellID) )
                currentUsersFollowList.follows.Add(wanttoFollow.CellID);
            Global.LocalStorage.SaveFollowList(currentUsersFollowList);
            return currentUsersFollowList;           
        }

        /// <summary>
        /// Returns user's and follows' posts on the wall 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="followList"></param>
        /// <returns></returns>
        public unsafe static List<Tweet> GetWall(User user)
        {
            List<Tweet> tempTweetList = new List<Tweet>(); 
            var  followls = Global.LocalStorage.FollowList_Selector().Where(fol => fol.user == user.CellID).FirstOrDefault(); //.Select(re => re.follows).FirstOrDefault();
            
            List<Tweet> usertweetList = Global.LocalStorage.Tweet_Selector().Where(tw => tw.user == user.CellID).ToList();
            if(usertweetList.Count() != 0)
            {
                tempTweetList.AddRange(usertweetList);
            }
            
            foreach (var follow in followls.follows)
            {                
               List<Tweet> tweetList = Global.LocalStorage.Tweet_Selector().Where(tw => tw.user == follow).ToList();
               if(tweetList.Count() != 0)
               {
                   tempTweetList.AddRange(tweetList);
               }
            }

            return tempTweetList;
        }

        /// <summary>
        /// Writes messages to the console
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="tweets"></param>
        public unsafe static void WriteTweetsToConsole( List<Tweet> tweets){
             foreach (Tweet tweetRecord in tweets.OrderByDescending(e => e.time))
            {
                Console.WriteLine(GetUser(tweetRecord.user).name + " - " + tweetRecord.text + " (" + (DateTime.UtcNow - tweetRecord.time).Minutes + " Minutes ago" + ")");
            }
        }
    }
}
