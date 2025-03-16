using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IMenuService
{
    List<Category> GetAllCategories();
    List<Modifiergroup> GetAllModifierGroups();
    Task<bool> AddCategory(Category category, long userId);
    Task<bool> EditCategory(Category category, long catID, long userId);
    Task<bool> DeleteCategory(long catID);
    PaginationViewModel<ItemsViewModel> GetItemsByCategory(long? catID, string search = "", int pageNumber = 1, int pageSize = 5);
    Task<bool> AddItem(AddItemViewModel addItemvm, long userId);
    AddItemViewModel GetItemByItemID(long itemID);
    Task<bool> EditItem(AddItemViewModel editvm, long userId);
    Task<bool> DeleteItem(long itemID);
    PaginationViewModel<ModifierViewModel> GetModifiersByModifierGrp(long? modifierGrpID, string search = "", int pageNumber = 1, int pageSize = 5);
    public PaginationViewModel<ModifierViewModel> GetAllModifiers( string search = "", int pageNumber = 1, int pageSize = 5);
     Task<bool> AddModifier(AddModifierViewModel addModifiervm, long userId);
    Task<bool> AddModifierGroup(AddModifierGroupViewModel modifiergrpvm , long userID);
    List<ModifierViewModel> GetModifiersByModifierGrpId(long ModGrpId);
    public Modifiergroup GetModifiergroupByGrpID(long ModGrpId);
     Task<bool> EditModifierGroup(AddModifierGroupViewModel modifiergrpvm, long userID);
     Task<bool> DeleteModifierFromModGrpAfterEdit(long modID,long modGrpID);
     Task<bool> AddModifierToModGrpAfterEdit(long modGrpID,long modifierID,long UserID);
    Task<bool> DeleteModifierGroup(long modGrpid);

}
