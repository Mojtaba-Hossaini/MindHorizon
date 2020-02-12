using MindHorizon.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MindHorizon.ViewModels.Post
{
    public class PostViewModel
    {
        [JsonProperty("Id")]
        public string PostId { get; set; }

        [JsonProperty("ردیف")]
        public int Row { get; set; }

        [JsonProperty("عنوان پست"), Display(Name = "عنوان پست")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Title { get; set; }

        [JsonProperty("ShortTitle")]
        public string ShortTitle { get; set; }

        [JsonIgnore]
        public bool FuturePublish { get; set; }

        [JsonIgnore]
        public DateTime? PublishDateTime { get; set; }

        [Display(Name = "تاریخ انتشار"), JsonProperty("تاریخ انتشار")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string PersianPublishDate { get; set; }

        [Display(Name = "زمان انتشار"), JsonIgnore]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string PersianPublishTime { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonProperty("نویسنده")]
        public string AuthorName { get; set; }

        [JsonIgnore]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "انتخاب {0} الزامی است.")]
        [JsonIgnore, Display(Name = "تصویر شاخص")]
        public string ImageFile { get; set; }

        [JsonIgnore]
        public bool IsPublish { get; set; }

        [JsonProperty("تگ ها")]
        public string NameOfTags { get; set; }


        [JsonProperty("بازدید")]
        public int NumberOfVisit { get; set; }

        [JsonProperty("NumberOfLike")]
        public int NumberOfLike { get; set; }

        [JsonProperty("NumberOfDisLike")]
        public int NumberOfDisLike { get; set; }

        [JsonProperty("دسته")]
        public string NameOfCategories { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "آدرس پست"), JsonProperty("آدرس")]
        public string Url { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }


        [JsonProperty("متن پست")]
        public string Description { get; set; }


        [JsonIgnore]
        public string[] CategoryIds { get; set; }

        [JsonIgnore]
        public PostCategoriesViewModel PostCategoriesViewModel { get; set; }

        [JsonIgnore]
        public virtual ICollection<PostCategory> PostCategories { get; set; }

        [JsonIgnore]
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
