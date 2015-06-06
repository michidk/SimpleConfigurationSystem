namespace SimpleConfigurationSystem.Example
{
    public class ExampleConfiguration : Configuration
    {

        public bool ExampleBool { get; set; }
        public int ExampleInt { get; set; }
        public string ExampleString { get; set; }

        public ExampleConfiguration() : base("example.yml") {}

        public override void LoadDefaults()
        {
            ExampleBool = false;
            ExampleInt = 1337;
            ExampleString = "Foo Bar";
        }
    }
}