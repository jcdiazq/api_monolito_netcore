namespace MonolitoApi.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UUID { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}