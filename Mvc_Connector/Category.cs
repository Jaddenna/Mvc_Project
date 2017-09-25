using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_Connector
{
	public class Category
	{
		//Cat Id  
		public int ID { get; set; }

		//Cat Name  
		public string Name { get; set; }

		//Cat Description  
		public string Description { get; set; }

		//represnts Parent ID and it's nullable  
		public int? Pid { get; set; }
		public virtual Category Parent { get; set; }
		public virtual ICollection<Category> Childs { get; set; }

		public static List<Category> ExCats
		{
			get
			{
				return new List<Category>()
				{
					new Category(){ID = 1, Name = "Cat1", Description="Desc1"},
					new Category(){ID = 2, Name = "Cat2", Description="Desc2"},
					new Category(){ID = 3, Name = "Cat3", Description="Desc3"},
					new Category(){ID = 4, Name = "Cat14", Description="Desc14", Pid = 1},
					new Category(){ID = 5, Name = "Cat5", Description="Desc5", Pid = 2},
					new Category(){ID = 5, Name = "Cat6", Description="Desc6", Pid = 2}
				};
			}
		}
	}
}
