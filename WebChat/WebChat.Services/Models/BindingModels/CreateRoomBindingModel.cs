namespace WebChat.Services.Models.BindingModels
{
    public class CreateRoomBindingModel
    {
        public string Password { get; set; }

        public string Type { get; set; }

        public int Size { get; set; }

        public string Name { get; set; }
    }
}