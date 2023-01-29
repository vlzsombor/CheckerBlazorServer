using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckerBlazorServer.Database.Dtos;
[Table("Test")]
public class TestDto
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
                
    public string Property { get; set; }

}

