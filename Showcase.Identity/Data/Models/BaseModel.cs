using System.ComponentModel.DataAnnotations;
using Showcase.Identity.Data.Constants;

namespace Showcase.Identity.Data.Models;

public class BaseViewModel;

public class BaseCustomizingViewModel : BaseViewModel
{
    public int Id { get; set; }
}

public class BaseCreateModel;

public class BaseLocalizeCreateModel : BaseCreateModel
{
    [Required]
    public string Text { get; set; } = null!;

    [Required]
    public string LanguageId { get; set; } = BootstrapperConstant.DefaultLanguage;
}

public class BaseOwnedCreateModel : BaseCreateModel
{
    public Guid ApplicationUserId { get; set; }
}

public class BaseUpdateModel;

public class BaseQueryFilterModel
{
    public bool? IsDeleted { get; set; }

    public int? PageSize { get; set; }
    
    public int? PageNumber { get; set; }
    
    public DateTime? BeginDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? NavigationalFilterKey { get; set; }
    
    public Guid? NavigationalFilterValue { get; set; }
}

public abstract class BaseCustomizingQueryFilterModel : BaseQueryFilterModel, ISortable
{
    public string? Text { get; set; }
    
    [AllowedValues("Text","Rate",null)]
    public string? SortParameter { get; set; }
    
    public bool? SortAscending { get; set; }
    
}

public interface ISortable
{
    string? SortParameter { get; set; }
    bool? SortAscending { get; set; }
}