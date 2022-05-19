using DiskShare.Models.AccountModels;
using DiskShare.Models.FileModels;
using DiskShare.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

namespace DiskShare.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public FileController(IWebHostEnvironment environment, SignInManager<AppUser> signIn,
            IHttpContextAccessor httpContext)
        {
            hostEnvironment = environment;
            signInManager = signIn;
            httpContextAccessor = httpContext;
        }

        private string GetUniqueFileName(string fileName)  //(zabezpieczenie) dodaje do nazwy pliku random GUID
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 9)
                      + Path.GetExtension(fileName);
        }


        [HttpGet]
        public IActionResult Disk(string id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                TempData["Fail"] = "Musisz być zalogowany aby korzystać z dysku!";
                return RedirectToAction("Index", "Home");
            }
            var getUserId = signInManager.UserManager.GetUserId(User);
            string projectPath = hostEnvironment.ContentRootPath;
            string userPath = projectPath + "UserDisks/";


            if (getUserId == id || id == null)
            {
                userPath = userPath + getUserId;
                if (!Directory.Exists(userPath))
                {
                    Directory.CreateDirectory(userPath);
                }
            }
            else
            {
                userPath = userPath + id;
                if (!Directory.Exists(userPath))
                {
                    ViewData["Fail"] = "Dysk o takim id nie istnieje!";
                    return RedirectToAction("Index", "Home");
                }

            }

            var folderWeight = Directory.GetFiles(userPath, "*", 
                SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));

            if (Directory.EnumerateFiles(userPath).Any())
            {
                var userFilesName = Directory.GetFiles(userPath);
                var fileInfo = new List<FileInfo>();
                foreach (var file in userFilesName)
                {
                    fileInfo.Add(new FileInfo(file));

                }

                #pragma warning disable CS8602 // Wyłuskanie odwołania, które może mieć wartość null.
                var url = httpContextAccessor.HttpContext.Request.GetDisplayUrl();
                #pragma warning restore CS8602 // Wyłuskanie odwołania, które może mieć wartość null.

                if (id == null)
                {
                    return View(new FileViewModel
                    {
                        isFilesExist = true,
                        isTheSameUsrId = true,
                        DiskCapacity = folderWeight,
                        DiskUrl = url  + "/" + getUserId,
                        Files = fileInfo
                    });
                }
                else if (getUserId == id)
                {
                    return View(new FileViewModel
                    {
                        isFilesExist = true,
                        isTheSameUsrId = true,
                        DiskCapacity = folderWeight,
                        DiskUrl = url,
                        Files = fileInfo
                    });
                }
                else
                {
                    return View(new FileViewModel
                    {
                        isFilesExist = true,
                        isTheSameUsrId = false,
                        DiskCapacity = folderWeight,
                        Files = fileInfo
                    });
                }
            }
            else
            {
                if (getUserId == id || id == null)
                {
                    return View(new FileViewModel
                    {
                        isFilesExist = false,
                        isTheSameUsrId = true,
                    });
                }
                else
                {
                    return View(new FileViewModel
                    {
                        isFilesExist = false,
                        isTheSameUsrId = false,
                    });
                }
                  
            }
        }

        [HttpPost]
        public IActionResult Disk(FileViewModel model)
        {
            if (model.newFile == null)
            {
                TempData["Fail"] = "Wybierz plik do przesłania!";
                return RedirectToAction("Disk", "File");
            }

            var getUserId = signInManager.UserManager.GetUserId(User);
            string projectPath = hostEnvironment.ContentRootPath;
            string userPath = projectPath + "UserDisks/" + getUserId;

            var folderWeight = model.DiskCapacity + model.newFile.formFile.Length;

            if (model.DiskCapacity > 40000000000)
            {
                TempData["Fail"] = "Nie masz wystarczająco miejsca na dysku! Usuń coś przed dodaniem następnego pliku";
                return RedirectToAction("Disk", "File");
            }


            if (model.newFile.formFile != null)
            {
                var uniqueFileName = GetUniqueFileName(model.newFile.formFile.FileName);
                var finalUserPath = Path.Combine(userPath, uniqueFileName);
                var stream = new FileStream(finalUserPath, FileMode.Create);
                model.newFile.formFile.CopyTo(stream);
                stream.Close();
                model.DiskCapacity = folderWeight;
                TempData["Success"] = "Pomyślnie przesłano plik na dysk";
            }

            return RedirectToAction("Disk", "File");
        }


        [HttpPost]
        public IActionResult Details(FileViewModel model)
        {
            byte[] img = System.IO.File.ReadAllBytes(model.FileFullName);
            var extension = Path.GetExtension(model.FileFullName);
            if (extension == ".jpg" || extension == ".jpeg")
            {
                return File(img, "image/png");
            }
            else
            {
                return File(img, "image/jpeg");
            }
            
        }

        [HttpPost]
        public IActionResult Download(FileViewModel model)
        {
            byte[] file = System.IO.File.ReadAllBytes(model.FileFullName);
            return File(file, "application/octet-stream", Path.GetFileName(model.FileFullName));
        }

        [HttpPost]
        public IActionResult Delete(FileViewModel model)
        {
            if (System.IO.File.Exists(model.FileFullName))
            {
                System.IO.File.Delete(model.FileFullName);
            }
            return RedirectToAction("Disk");
        }
    }
}
