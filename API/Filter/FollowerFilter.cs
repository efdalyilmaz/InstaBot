using InstaBot.API.Utils;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaBot.API.Filter
{
    public sealed class FollowerFilter : IFilter<InstaUserShort>
    {
        private bool? IsPrivate;
        private bool? IsKnownProfile;
        private bool IsCheckedRequested;
        private int MaxFollowerCount = ApiConstans.MAX_FOLLOWER_REQUEST_COUNT;

        public FollowerFilter SetPrivate(bool isPrivate)
        {
            IsPrivate = isPrivate;
            return this;
        }

        public FollowerFilter SetKnownProfile(bool isKnownProfile)
        {
            IsKnownProfile = isKnownProfile;
            return this;
        }

        public FollowerFilter SetMaxFollowerCount(int maxFollowerCount)
        {
            MaxFollowerCount = maxFollowerCount;
            return this;
        }

        public FollowerFilter CheckRequested()
        {
            IsCheckedRequested = true;
            return this;
        }

        public List<InstaUserShort> Apply(List<InstaUserShort> list)
        {
            Console.WriteLine(String.Format("Firstly, Follower User Count : {0}", list.Count));

            List<InstaUserShort> filtered = list;
            if (IsPrivate != null)
            {
                filtered= filtered.FindAll(p => p.IsPrivate == IsPrivate.GetValueOrDefault());
                Console.WriteLine(String.Format("IsPrivate Filter, Follower User Count : {0}", filtered.Count));
            }

            if (IsKnownProfile != null)
            {
                if (IsKnownProfile.GetValueOrDefault())
                {
                    filtered = filtered.FindAll(p => p.ProfilePictureId != ApiConstans.UNKNOWN);
                }
                else
                {
                    filtered = filtered.FindAll(p => p.ProfilePictureId == ApiConstans.UNKNOWN);
                }

                Console.WriteLine(String.Format("IsKnownProfile Filter, Follower User Count : {0}", filtered.Count));
            }

            if(IsCheckedRequested)
            {
                var requestedUser = FileUtils.ReadRequestedList();
                filtered.RemoveAll(item => requestedUser.Contains(item.Pk));

                Console.WriteLine(String.Format("CheckRequested Filter, Follower User Count : {0}", filtered.Count));
            }

            filtered = filtered.Take(MaxFollowerCount).ToList();
            Console.WriteLine(String.Format("Finally, Follower User Count : {0}", filtered.Count));

            return filtered;
        }

        public static FollowerFilter DefaultFilter()
        {
            FollowerFilter filter = FollowerFilter.CreateFilter().
                SetPrivate(true).
                SetKnownProfile(true).
                CheckRequested();

            return filter;
        }

        public static FollowerFilter CreateFilter()
        {
            return new FollowerFilter();
        }


    }
}
