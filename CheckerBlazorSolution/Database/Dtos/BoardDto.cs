using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CheckerBlazorServer.Database.Dtos;
[Table("Test")]
public class BoardDto 
{
    public string TableId { get; set; }
    public string TableJson { get; set; }
}

