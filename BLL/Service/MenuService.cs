using System.Numerics;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BLL.Service;

public class MenuService : IMenuService
{
    private readonly PizzashopDbContext _context;

    public MenuService(PizzashopDbContext context)
    {
        _context = context;
    }

    public List<Category> GetAllCategories()
    {
        return _context.Categories.Where(x => x.Isdelete == false).ToList();
    }

    public List<Modifiergroup> GetAllModifierGroups(){
        return _context.Modifiergroups.Where(x=>x.Isdelete==false).ToList();
    }

    public async Task<bool> AddCategory(Category category, long userId)
    {
        var ispresentcat = await _context.Categories.FirstOrDefaultAsync(x =>x.Isdelete==false && x.CategoryName == category.CategoryName);
        if (ispresentcat != null)
        {
            return false;
        }
        if (category == null)
        {
            return false;
        }
        Category cat = new Category();
        cat.CategoryName = category.CategoryName;
        cat.Description = category.Description;
        cat.CreatedBy = userId;
        await _context.Categories.AddAsync(cat);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> EditCategory(Category category, long catID, long userId)
    {
        if (category == null || catID == null)
        {
            return false;
        }
        Category cat = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catID);
        cat.CategoryName = category.CategoryName;
        cat.Description = category.Description;
        cat.ModifiedBy = userId;
        cat.ModifiedAt = DateTime.Now;
        _context.Update(cat);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCategory(long catID)
    {
        if (catID == null)
        {
            return false;
        }
        Category category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == catID);
        category.Isdelete = true;
        _context.Update(category);
        await _context.SaveChangesAsync();
        return true;


    }


    public PaginationViewModel<ItemsViewModel> GetItemsByCategory(long? catID, string search = "", int pageNumber = 1, int pageSize = 5)
    {

        var query = _context.Items
            .Include(x => x.Category).Include(x => x.ItemType)
            .Where(x => x.CategoryId == catID).Where(x => x.Isdelete == false)
            .Select(x => new ItemsViewModel
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                CategoryId = x.CategoryId,
                ItemTypeId = x.ItemTypeId,
                TypeImage = x.ItemType.TypeImage,
                Rate = x.Rate,
                Quantity = x.Quantity,
                ItemImage = x.ItemImage,
                Isavailable = x.Isavailable,
                Isdelete = x.Isdelete
            })
            .AsQueryable();

        //search 
        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearchTerm = search.ToLower();
            query = query.Where(x =>
                x.ItemName.ToLower().Contains(lowerSearchTerm)
            );
        }

        // Get total records count (before pagination)
        int totalCount = query.Count();

        // Apply pagination
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<ItemsViewModel>(items, totalCount, pageNumber, pageSize);
    }


    public async Task<bool> AddItem(AddItemViewModel addItemvm, long userId)
    {
        if (addItemvm.CategoryId == null)
        {
            return false;
        }
        Item item = new();
        item.CategoryId = addItemvm.CategoryId;
        item.ItemName = addItemvm.ItemName;
        item.ItemTypeId = addItemvm.ItemTypeId;
        item.Rate = addItemvm.Rate;
        item.Quantity = addItemvm.Quantity;
        item.Unit = addItemvm.Unit;
        item.Isavailable = addItemvm.Isavailable;
        item.Isdefaulttax = addItemvm.Isdefaulttax;
        item.TaxValue = addItemvm.TaxValue;
        item.ShortCode = addItemvm.ShortCode;
        item.Description = addItemvm.Description;
        item.ItemImage = addItemvm.ItemImage;
        item.CreatedBy = userId;

        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        return true;
    }


    public AddItemViewModel GetItemByItemID(long itemID){
        var items = _context.Items.Where(x=>x.ItemId==itemID && x.Isdelete==false).ToList();
        AddItemViewModel editItemvm = new AddItemViewModel()
        {
            CategoryId = items[0].CategoryId,
            ItemId =items[0].ItemId,
            ItemName=items[0].ItemName,
            ItemTypeId = items[0].ItemTypeId,
            Rate = items[0].Rate,
            Quantity = items[0].Quantity,
            Unit = items[0].Unit,
            Isavailable = (bool)items[0].Isavailable,
            Isdefaulttax = (bool)items[0].Isdefaulttax,
            TaxValue = items[0].TaxValue,
            ShortCode = items[0].ShortCode,
            Description=items[0].Description,
        };
        // editItemvm.CategoryId = items[0].CategoryId;
        // Assign other properties as needed
        return editItemvm;
    }

    public async Task<bool> EditItem(AddItemViewModel editvm, long userId){
        Item item=await _context.Items.FirstOrDefaultAsync(x=>x.ItemId==editvm.ItemId);
        if(item == null){
            return false;
        }
        item.CategoryId = editvm.CategoryId;
        item.ItemName=editvm.ItemName;
        item.ItemTypeId =editvm.ItemTypeId;
        item.Rate=editvm.Rate;
        item.Quantity=editvm.Quantity;
        item.Unit=editvm.Unit;
        item.Isavailable=editvm.Isavailable;
        item.Isdefaulttax=editvm.Isdefaulttax;
        item.TaxValue=editvm.TaxValue;
        item.ShortCode=editvm.ShortCode;
        item.Description=editvm.Description;
        item.ItemImage = editvm.ItemImage;
        item.ModifiedBy=userId;
        item.ModifiedAt=DateTime.Now;
        _context.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItem(long itemID)
    {
        if (itemID == null)
        {
            return false;
        }
        Item item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == itemID);
        item.Isdelete = true;
        _context.Update(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public PaginationViewModel<ModifierViewModel> GetModifiersByModifierGrp(long? modifierGrpID, string search = "", int pageNumber = 1, int pageSize = 5)
    {
        var query = _context.Modifiers
        .Include(x=>x.ModifierGrp)
            .Where(x => x.ModifierGrpId == modifierGrpID).Where(x => x.Isdelete == false)
            .Select(x => new ModifierViewModel
            {
                ModifierGrpId=x.ModifierGrpId,
                ModifierId=x.ModifierId,
                ModifierName=x.ModifierName,
                Rate=x.Rate,
                Quantity=x.Quantity,
                Unit=x.Unit,
                Isdelete = x.Isdelete
            })
            .AsQueryable();

        //search 
        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearchTerm = search.ToLower();
            query = query.Where(x =>
                x.ModifierName.ToLower().Contains(lowerSearchTerm)
            );
        }

        // Get total records count (before pagination)
        int totalCount = query.Count();

        // Apply pagination
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<ModifierViewModel>(items, totalCount, pageNumber, pageSize);
    }

    public PaginationViewModel<ModifierViewModel> GetAllModifiers( string search = "", int pageNumber = 1, int pageSize = 5)
    {
        var query = _context.Modifiers.Where(x => x.Isdelete == false)
        .Select(x => new ModifierViewModel
            {
                ModifierGrpId=x.ModifierGrpId,
                ModifierId=x.ModifierId,
                ModifierName=x.ModifierName,
                Rate=x.Rate,
                Quantity=x.Quantity,
                Unit=x.Unit,
                Isdelete = x.Isdelete
            })
            .AsQueryable();

        //search 
        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearchTerm = search.ToLower();
            query = query.Where(x =>
                x.ModifierName.ToLower().Contains(lowerSearchTerm)
            );
        }

        // Get total records count (before pagination)
        int totalCount = query.Count();

        // Apply pagination
        var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<ModifierViewModel>(items, totalCount, pageNumber, pageSize);
    }


    public async Task<bool> AddModifier(AddModifierViewModel addModifiervm, long userId){
        if(addModifiervm.ModifierGrpId==null)
        {
             return false;
        }
        Modifier modifier=new();
        modifier.ModifierGrpId=addModifiervm.ModifierGrpId;
        modifier.ModifierName=addModifiervm.ModifierName;
        modifier.Rate=addModifiervm.Rate;
        modifier.Quantity=addModifiervm.Quantity;
        modifier.Unit=addModifiervm.Unit;
        modifier.Description=addModifiervm.Description;
        modifier.CreatedBy=userId;

       await  _context.AddAsync(modifier);
        _context.SaveChanges();
        return true;

    }

    public async Task<bool> AddModifierGroup(AddModifierGroupViewModel modifiergrpvm , long userID)
    {
        var presentModifiergrp =await _context.Modifiergroups.FirstOrDefaultAsync(x=>x.ModifierGrpName == modifiergrpvm.ModifierGrpName && x.Isdelete==false);

        if(presentModifiergrp !=null){
            return false;

        }
        Modifiergroup modgrp = new();
        modgrp.ModifierGrpName = modifiergrpvm.ModifierGrpName;
        modgrp.Desciption = modifiergrpvm.Desciption;
        modgrp.CreatedBy = userID;
        
        await _context.AddAsync(modgrp);
         _context.SaveChanges();
        
        var modifiergrpadded= await _context.Modifiergroups.FirstOrDefaultAsync(x=>x.ModifierGrpName==modifiergrpvm.ModifierGrpName && x.Isdelete==false);

        var modTempID = modifiergrpvm.tempIds.Split(",");
        for(int i=0; i<modTempID.Length;i++){
            var modifierExist =await  _context.Modifiers.FirstOrDefaultAsync(x=>x.ModifierId == int.Parse(modTempID[i]));
            Modifier modifier = new();
            modifier.ModifierGrpId = modifiergrpadded.ModifierGrpId;
            modifier.ModifierName = modifierExist.ModifierName;
            modifier.Description=modifierExist.Description;
            modifier.Rate=modifierExist.Rate;
            modifier.Quantity=modifierExist.Quantity;
            modifier.Unit=modifierExist.Unit;
            modifier.CreatedBy = userID;
            await _context.AddAsync(modifier);
            _context.SaveChanges();
        }
    
        return true;
    }

    public  List<ModifierViewModel> GetModifiersByModifierGrpId(long ModGrpId){
        var modifiers = _context.Modifiers.Where(x=>x.ModifierGrpId==ModGrpId && x.Isdelete==false)
        .Select(x=>new ModifierViewModel{
            ModifierGrpId=x.ModifierGrpId,
            ModifierId=x.ModifierId,
            ModifierName=x.ModifierName,
            Rate=x.Rate,
            Quantity=x.Quantity,
            Unit=x.Unit,
            Isdelete = x.Isdelete
        }).ToList();
        return modifiers;
    }

    public Modifiergroup GetModifiergroupByGrpID(long ModGrpId)
    {
        var modifiergrp = _context.Modifiergroups.FirstOrDefault(x=>x.ModifierGrpId==ModGrpId && x.Isdelete==false);
        return modifiergrp;
    }

    public async Task<bool> EditModifierGroup(AddModifierGroupViewModel modifiergrpvm, long userID)
    {
        var presentmodifierGroup =await _context.Modifiergroups.FirstOrDefaultAsync(x=>x.ModifierGrpId==modifiergrpvm.ModifierGrpId);
        if(presentmodifierGroup ==null){
            return false;
        }
        presentmodifierGroup.ModifierGrpName = modifiergrpvm.ModifierGrpName;
        presentmodifierGroup.Desciption = modifiergrpvm.Desciption;
        presentmodifierGroup.ModifiedBy = userID;
        presentmodifierGroup.ModifiedAt=DateTime.Now;
        _context.Modifiergroups.Update(presentmodifierGroup);
        await _context.SaveChangesAsync();
        return true;
       
    }

    public async Task<bool> DeleteModifierFromModGrpAfterEdit(long modGrpID,long modifierID){
        var existingmodifier =await  _context.Modifiers.FirstOrDefaultAsync(x=>x.ModifierId==modifierID && x.ModifierGrpId==modGrpID );
        if(existingmodifier==null){
            return false;
        }
        existingmodifier.Isdelete = true;
        _context.Modifiers.Update(existingmodifier);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> AddModifierToModGrpAfterEdit(long modGrpID,long modifierID,long UserID){
        var data = await _context.Modifiers.Where(x => x.ModifierId == modifierID && x.Isdelete == false).ToListAsync();
        Modifier modifier = new(){
            ModifierGrpId = modGrpID,
            Rate = data[0].Rate,
            ModifierName = data[0].ModifierName,
            Quantity = data[0].Quantity,
            Unit = data[0].Unit,
            Description = data[0].Description,
            CreatedBy = UserID
        };
        await _context.AddAsync(modifier);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<bool> DeleteModifierGroup(long modGrpid)
    {
        Modifiergroup modifierGroupToDelete =await _context.Modifiergroups.FirstOrDefaultAsync(x=>x.ModifierGrpId == modGrpid);
        List<Modifier> existingModifiers = _context.Modifiers.Where(x=>x.ModifierGrpId==modGrpid).ToList();

        for(int i=0;i<existingModifiers.Count;i++){
            existingModifiers[i].Isdelete=true;
             _context.Update(existingModifiers[i]);
             _context.SaveChanges();
        }

        modifierGroupToDelete.Isdelete=true;
        _context.Update(modifierGroupToDelete);
        _context.SaveChanges();
        return true;
    }



}