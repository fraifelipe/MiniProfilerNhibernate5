using System;
using FluentNHibernate.Mapping;

namespace MiniProfilerNhibernate5.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
    }
    
    public class TodoMap: ClassMap<Todo>
    {
        public TodoMap()
        {
            Schema("dbo");
            Table("TodoCollection");
            
            Id(x => x.Id).GeneratedBy.GuidComb();

            Map(x => x.Name);
            Map(x => x.Complete);
        }
    }
}