namespace DynamicsEntityGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableNameAttribute : System.Attribute
    {
        public string Name { get; }

        public TableNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
