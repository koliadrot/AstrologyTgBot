namespace Data.Core.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Client
    {
        [Column("ClientId", Order = 1)]
        public int ClientId { get; set; }

        [DisplayName("Id телеграмм аккаунта")]
        public string? TelegramId { get; set; }

        [DisplayName("Техническое имя")]
        public string? UserName { get; set; }

        [DisplayName("Имя")]
        [MaxLength(64)]
        public string? FirstName { get; set; }

        [DisplayName("Пол рождения")]
        public string? BirthGender { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Координаты место рождения")]
        public string? BirthCoord { get; set; }

        [DisplayName("Город рождения")]
        public string? BirthCity { get; set; }

        [DisplayName("Пол поиска")]
        public string? SearchGender { get; set; }

        [DisplayName("Возраст поиска")]
        public string? SearchAge { get; set; }

        [DisplayName("Город поиска")]
        public string? SearchCity { get; set; }

        [DisplayName("Координаты поиска")]
        public string? SearchCoord { get; set; }

        [DisplayName("Возраст поиска")]
        public string? SearchGoal { get; set; }

        [DisplayName("Описание")]
        public string? AboutMe { get; set; }

        [DisplayName("Медиа")]
        public virtual ClientMediaInfo ClientMediaInfo { get; set; }
    }
}
