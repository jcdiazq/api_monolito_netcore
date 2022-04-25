namespace MonolitoApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public virtual Image Image { get; set; }
    }
}