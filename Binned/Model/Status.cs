namespace Binned.Model
{
    public class Status
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public static IEnumerable<Status> Statuses = new List<Status> {
            new Status {
                Value = "To Pay",
                Name = "To Pay"
            },
            new Status {
                Value = "To Ship",
                Name = "To Ship"
            },
            new Status {
                Value = "To Receive",
                Name = "To Receive"
            },
            new Status {
                Value = "Delivered",
                Name = "Delivered"
            }
        };

    }
}
