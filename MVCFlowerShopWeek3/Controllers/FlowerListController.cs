using Microsoft.AspNetCore.Mvc;
using MVCFlowerShopWeek3.Models;
using MVCFlowerShopWeek3.Data;
using Microsoft.EntityFrameworkCore;

namespace MVCFlowerShopWeek3.Controllers
{
    public class FlowerListController : Controller
    {
        private readonly MVCFlowerShopWeek3Context _context;

        public FlowerListController(MVCFlowerShopWeek3Context context)
        {
            _context = context;
        }

        //Function 1: Learn how to crete the add flower form

        public IActionResult AddNewFlower()
        {
            return View();
        }

        //Funciton 2: Learn how to insert ot the flower table
        [HttpPost]
        [ValidateAntiForgeryToken] // avoid cross-site attack

        public async Task<IActionResult> AddFlower(Flower flower)
        {
            if (ModelState.IsValid)
            {
                _context.FlowerTable.Add(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FlowerList");
            }
            return View("AddNewFlower", flower);
        }

        //Function 3: Learn how to retreive data back from Flower Table

        public async Task<IActionResult> Index(String searchString)
        {
            List<Flower> flowerList = await _context.FlowerTable.ToListAsync();

            //filter the flower before display
            if (!string.IsNullOrEmpty(searchString))
            {
                flowerList = flowerList.Where(s => s.FlowerName.Contains(searchString)).ToList();
            }

            return View(flowerList);
        }

        //function 4: learn how to delete item from the flower table
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> deletepage(int ? fid)
        {
            if (fid == null)
                return NotFound();

            Flower flower = await _context.FlowerTable.FindAsync(fid);

            if (flower == null)
                return NotFound();

            _context.FlowerTable.Remove(flower);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FlowerList");
        }

        //function 5: learn how to edit item from the flower table
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public async Task<IActionResult> editpage(int ? fid) 
        {
            if (fid == null)
                return NotFound();

            Flower flower = await _context.FlowerTable.FindAsync(fid);
            if (flower == null)
                return NotFound();
            
            return View(flower);


        }

        //Function 6: Learn how to update the form
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>updatepage(Flower flower)
        {
            if(ModelState.IsValid)
            {
                _context.FlowerTable.Update(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FlowerList");
            }
            return View("editpage", flower);
        }


	}
}
