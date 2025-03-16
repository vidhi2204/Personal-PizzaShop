using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Service;
using BLL.Interfaces;

// using BLL.Services;

using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace Pizzashop_Project.Controllers;
[Authorize(Roles = "Admin")]
public class MenuController : Controller
{

    private readonly IMenuService _menuService;

    private readonly IUserLoginService _userLoginSerivce;

    private readonly IUserService _userService;

    // private readonly UserService _userservice;

    public MenuController(IMenuService menuService, IUserLoginService userLoginSerivce, IUserService userService)
    {
        _menuService = menuService;
        _userLoginSerivce = userLoginSerivce;
        _userService = userService;
    }

    #region  Menu get
    public IActionResult Menu(long? catID, long? modifierGrpID, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        MenuViewModel menudata = new();

        // categories----------------------
        menudata.categories = _menuService.GetAllCategories();
        if (catID == null)
        {
            // menudata.itemList = _menuService.GetItemsByCategory(-100).Items;
            // ViewBag.catSelect = menudata.categories[0].CategoryId;
            menudata.Pagination = _menuService.GetItemsByCategory(menudata.categories[0].CategoryId, search, pageNumber, pageSize);
        }

        // if (catID != null)
        // {
        //     // ViewBag.catSelect = catID;
        //     menudata.Pagination = _menuService.GetItemsByCategory(catID, search, pageNumber, pageSize);
        // }

        // modifiers---------------------------
        menudata.modifiergroupList = _menuService.GetAllModifierGroups();
        if (modifierGrpID == null)
        {
            menudata.Paginationmodifiers = _menuService.GetModifiersByModifierGrp(menudata.modifiergroupList[0].ModifierGrpId, search, pageNumber, pageSize);
        }
        // if(modifierGrpID != null)
        // {
        //     menudata.Paginationmodifiers = _menuService.GetModifiersByModifierGrp(modifierGrpID,search,pageNumber,pageSize);
        // }

        ViewData["sidebar-active"] = "Menu";
        return View(menudata);
    }
    #endregion

    #region menniItemPagination
    public IActionResult MenuItemPagination(long? catID, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        MenuViewModel menudata = new();
        menudata.categories = _menuService.GetAllCategories();

        if (catID != null)
        {
            menudata.Pagination = _menuService.GetItemsByCategory(catID, search, pageNumber, pageSize);
        }
        return PartialView("_ItemListPartal", menudata.Pagination);
    }
    #endregion

    #region menu modifiers Pagination
    public IActionResult MenuModifierPagination(long? modifierGrpID, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        MenuViewModel menudata = new();
        menudata.modifiergroupList = _menuService.GetAllModifierGroups();

        if (modifierGrpID != null)
        {
            menudata.Paginationmodifiers = _menuService.GetModifiersByModifierGrp(modifierGrpID, search, pageNumber, pageSize);
        }
        return PartialView("_ModifierListPartial", menudata.Paginationmodifiers);
    }
    #endregion

       #region menu modifiers Pagination
    public IActionResult MenuModifierAllPagination( string search = "", int pageNumber = 1, int pageSize = 5)
    {
        MenuViewModel menudata = new();
        menudata.modifiergroupList = _menuService.GetAllModifierGroups();

            menudata.Paginationmodifiers = _menuService.GetAllModifiers(search, pageNumber, pageSize);
       
        return PartialView("_AddExisingModifierPaginationPartial", menudata.Paginationmodifiers);
    }
    #endregion

    #region menu list of modifier group get
    public IActionResult GetAllModifierGroups(){
        MenuViewModel menudata = new();
        menudata.modifiergroupList = _menuService.GetAllModifierGroups();
        return PartialView("_ModifierGroupListPartial",menudata);
    }
    #endregion


    #region Add Category 
    public async Task<IActionResult> AddCategory(MenuViewModel menuvm)
    {
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        bool addcategoryStatus = await _menuService.AddCategory(menuvm.category, userId);
        if (addcategoryStatus)
        {
            TempData["SuccessMessage"] = "Category Added Successfully";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Failed to add Category. Try Again";
        return RedirectToAction("Menu");
    }
    #endregion

    #region EditCategory
    public async Task<IActionResult> EditCategory(MenuViewModel menuvm)
    {
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        var catID = menuvm.category.CategoryId;
        bool editCategoryStatus = await _menuService.EditCategory(menuvm.category, catID, userId);
        if (editCategoryStatus)
        {
            TempData["SuccessMessage"] = "Category Updated Successfully";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Failed to Update Category. Try Again";
        return RedirectToAction("Menu");

    }
    #endregion


    #region delete category
    public async Task<IActionResult> DeleteCategory(long id)
    {
        var CategoryDeleteStatus = await _menuService.DeleteCategory(id);
        if (CategoryDeleteStatus)
        {
            TempData["SuccessMessage"] = "Category Deleted Successfully";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Failed to delete Category. Try Again";
        return RedirectToAction("Menu");
    }
    #endregion


    #region AddItems
    [HttpPost]
    public async Task<IActionResult> AddItem(MenuViewModel addItemViewModel)
    {
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);

        if (addItemViewModel.additem.ItemFormImage != null)
        {

            var extension = addItemViewModel.additem.ItemFormImage.FileName.Split(".");
            if (extension[extension.Length - 1] == "jpg" || extension[extension.Length - 1] == "jpeg" || extension[extension.Length - 1] == "png")
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = $"{Guid.NewGuid()}_{addItemViewModel.additem.ItemFormImage.FileName}";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    addItemViewModel.additem.ItemFormImage.CopyTo(stream);
                }
                addItemViewModel.additem.ItemImage = $"/uploads/{fileName}";
            }
            else
            {
                TempData["ErrorMessage"] = "Please Upload an Image in JPEG, PNG or JPG format.";
                return RedirectToAction("AddItem", "Menu");
            }
        }

        var addItemStatus = await _menuService.AddItem(addItemViewModel.additem, userId);
        if (addItemStatus)
        {
            TempData["SuccessMessage"] = "Item Added SuccessFully.";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Error while ItemAdd. Try Again..";
        return RedirectToAction("Menu");
    }
    #endregion


    #region  edit item Get
    public IActionResult EditItem(long itemID)
    {
        return Json(_menuService.GetItemByItemID(itemID));
    }
    #endregion


    #region  edititem post
    [HttpPost]
    public async Task<IActionResult> EditItem(MenuViewModel menuvm)
    {
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        if (menuvm.additem.ItemFormImage != null)
        {
            var extension = menuvm.additem.ItemFormImage.FileName.Split(".");
            if (extension[extension.Length - 1] == "jpg" || extension[extension.Length - 1] == "jpeg" || extension[extension.Length - 1] == "png")
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = $"{Guid.NewGuid()}_{menuvm.additem.ItemFormImage.FileName}";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    menuvm.additem.ItemFormImage.CopyTo(stream);
                }
                menuvm.additem.ItemImage = $"/uploads/{fileName}";
            }
            else
            {
                TempData["ErrorMessage"] = "Please Upload an Image in JPEG, PNG or JPG format.";
                return RedirectToAction("AddItem", "Menu");
            }
        }
        if (await _menuService.EditItem(menuvm.additem, userId))
        {
            TempData["SuccessMessage"] = "Item Updated Successfully";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Failed to Update Item. Try Again!";
        return RedirectToAction("Menu");

    }
    #endregion

    #region delete item
    public async Task<IActionResult> DeleteItem(long itemID)
    {
        var CategoryDeleteStatus = await _menuService.DeleteItem(itemID);
        if (CategoryDeleteStatus)
        {
            TempData["SuccessMessage"] = "Category Deleted Successfully";
            return RedirectToAction("Menu");
        }
        TempData["ErrorMessage"] = "Failed to delete Category. Try Again";
        return RedirectToAction("Menu");
    }
    #endregion

    #region AddModifier get
    public IActionResult AddModifierModal()
    {
        MenuViewModel menuvm=new MenuViewModel();
         menuvm.modifiergroupList = _menuService.GetAllModifierGroups();
        return PartialView("_AddModifierModal", menuvm);
    }
    #endregion


    #region Add Modifier post
    [HttpPost]
    public async Task<IActionResult> AddModifier([FromForm] MenuViewModel menuvm)
    {
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);

        var addItemStatus = await _menuService.AddModifier(menuvm.addModifier, userId);
        if (addItemStatus)
        {
            TempData["SuccessMessage"] = "Item Added SuccessFully.";
            return Json("done");
        }
        TempData["ErrorMessage"] = "Error while ItemAdd. Try Again..";
        return Json(" not done");
    }
    #endregion

    // #region AddModifierGroup get
    // public IActionResult AddModifierGroup(){
    //     MenuViewModel menuvm=new MenuViewModel();
    //     //  menuvm.modifiergroupList = _menuService.GetAllModifierGroups();
    //     return PartialView("_AddModifierGroupPartial", menuvm);
    // }    
    // #endregion

    #region AddModifierGroup post
    [HttpPost]
    public async Task<IActionResult> AddModifierGroup(MenuViewModel menuvm ){
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        var addModifierGrpStatus =await  _menuService.AddModifierGroup(menuvm.addmodgrpVm,userId);
        if(addModifierGrpStatus){
            return Json("modifier added");
        }
        return Json("not added");
    }
    #endregion

    #region Edit Modifier Group get
    public IActionResult EditModGrp(long ModGrpId){
        MenuViewModel menuvm = new MenuViewModel();
        var modifiers = _menuService.GetModifiersByModifierGrpId(ModGrpId);
        var modifierGroup = _menuService.GetModifiergroupByGrpID(ModGrpId);

        return Json(new{modifiers, modifierGroup});

    }
    #endregion

    #region editmod grp post
    [HttpPost]
    public async Task<IActionResult> EditModifierGroup(MenuViewModel menuvm){
        string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        var editModStatus=await _menuService.EditModifierGroup(menuvm.addmodgrpVm,userId);
        if(editModStatus){
            return Json(menuvm.addmodgrpVm.ModifierGrpId);
        }
        else{
            return Json("modifier group data not updated successfully");
        }
    }
    #endregion

    //deleteModifierFromModGrpAfterEdit?modGrpID=${modGrpID}&modifierID=${editModTempId[i]}
    #region deleteModifierFromModGrpAfterEdit post
    [HttpPost]
    public async Task<IActionResult> DeleteModifierFromModGrpAfterEdit(long modGrpID,long modifierID){
        var deleteModStatus =await  _menuService.DeleteModifierFromModGrpAfterEdit(modGrpID,modifierID);
        if(deleteModStatus){
        return Json("existing modifier deleted success while edit mod grp");
        }
        return Json("fail to delete existing mod in modgrp while edit");
    }
    #endregion

//addModifierToModGrpAfterEdit?modGrpID=${modGrpID}&modifierID=${modTempID[i]}
    #region addModifierToModGrpAfterEdit post
    [HttpPost]
    public async Task<IActionResult> addModifierToModGrpAfterEdit(long modGrpID,long modifierID ){
         string token = Request.Cookies["AuthToken"];
        var userData = _userService.getUserFromEmail(token);
        long userId = _userLoginSerivce.GetUserId(userData[0].Userlogin.Email);
        var addModStatus =await _menuService.AddModifierToModGrpAfterEdit(modGrpID,modifierID,userId);
        if(addModStatus){
            return Json("existing modifier added success while edit mod grp");
        }
        return Json("fail to add existing mod in modgrp while edit");
    }
    #endregion


    #region Delete Mod Grp
    [HttpPost]
    public async Task<IActionResult> DeleteModGrp(long modGrpid)
    {
        var deletemodgrpStatus =await _menuService.DeleteModifierGroup(modGrpid);
        if(deletemodgrpStatus){
            return Json("modifier group dleted");
        }
        return Json("modifier group not deleted");
    }
    #endregion


}