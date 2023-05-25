using System.ComponentModel;
using System.Linq;

namespace UnitGPT.Options.Converter
{
    public class ProjectNameListConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var projects = VS.Solutions.GetAllProjectsAsync().Result;
            return new StandardValuesCollection(projects.Select(p => p.Name).ToList());
        }
    }
}
