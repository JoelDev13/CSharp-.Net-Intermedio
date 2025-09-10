using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformSchool.Domain.Models.Department
{
	public record DepartmentUpdateModel : DepartmentModel
	{
		public int DepartmentId { get; set; }
		public int UserMod { get; set; }
		public DateTime ModifyDate { get; set; }

	}
}
