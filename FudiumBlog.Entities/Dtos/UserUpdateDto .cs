﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Entities.Dtos
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} Boş Geçilmemelidir.")]//display name de dne yazıyorsa {0}buraya eklenicek.
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]//dinamik kullandık tehrar tekrar yazmamak için
        [MinLength(3, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]
        public string UserName { get; set; }
        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} Boş Geçilmemelidir.")]//display name de dne yazıyorsa {0}buraya eklenicek.
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]//dinamik kullandık tehrar tekrar yazmamak için
        [MinLength(10, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
      
        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "{0} Boş Geçilmemelidir.")]//display name de dne yazıyorsa {0}buraya eklenicek.
        [MaxLength(13, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]//dinamik kullandık tehrar tekrar yazmamak için
        [MinLength(13, ErrorMessage = "{0} {1} karakterden küçük olmamalıdır.")]//+901234567891
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DisplayName("Resim Ekle")]
        [DataType(DataType.Upload)]
        public IFormFile PictureFile { get; set; }
        [DisplayName("Resim")]
        public string Picture { get; set; }
    }
}
