using System.Collections.Generic;

namespace InstaBot.Filter
{
    public interface IFilter<T>
    {

        List<T> Apply(List<T> list);
    }
}
