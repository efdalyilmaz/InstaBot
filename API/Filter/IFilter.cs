using System.Collections.Generic;

namespace InstaBot.API.Filter
{
    public interface IFilter<T>
    {

        List<T> Apply(List<T> list);
    }
}
