using AutoMapper;
using System;
using CS_DAL.Entities;
using CS_DAL.Interfaces;
using CS_BLL.Models;
using CS_BLL.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CS_BLL.Services
{
    public class FileService
    {
        readonly IFileDataRepository fileDataRepository;
        readonly IUnitOfWork unitOfWork;
        readonly IMapper mapper;

        public FileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            fileDataRepository = unitOfWork.FileDataRepository;
        }

        public static string GetFilePath(FileDatumModel file)
        {
            StringBuilder sb = new(GetDirectoryPath(file.UserName));
            sb.Append('\\');
            sb.Append(file.FileName);
            return sb.ToString();
        }

        public static string GetDirectoryPath(string userName)
        {
            StringBuilder sb = new(@"D:\MyWorks\.NET\Final_project_w2022\CloudStore\CS_DAL\Storage\");
            sb.Append(userName);
            return sb.ToString();
        }

        public static string GetFilePath(FileDatum file)
        {
            StringBuilder sb = new(GetDirectoryPath(file.UserName));
            sb.Append('\\');
            sb.Append(file.FileName);
            return sb.ToString();
        }

        public async Task<FileDatum> GetNewEntityFromModel(FileDatumModel model)
        {
            var res = mapper.Map<FileDatumModel, FileDatum>(model);
            return res;
        }

        public async Task<int> GetIdByModel(FileDatumModel model)
        {
            return (await fileDataRepository.GetAllAsync()).SingleOrDefault(fd => fd.UserName == model.UserName && fd.FileName == model.FileName).Id;
        }

        public FileDatumModel GetModelFromEntity(FileDatum entity)
        {
            var res = mapper.Map<FileDatum, FileDatumModel>(entity);
            System.IO.FileInfo info = new(GetFilePath(entity));
            res.Size = info.Length;
            return res;
        }

        async Task<bool> CheckExistanceInDB(string userName, string fileName)
        {
            return (await fileDataRepository.GetAllAsync()).Any(fd => fd.UserName == userName && fd.FileName == fileName);
        }

        static bool CheckViaFilter(FileDatum entity, FilterFileDatumModel filter)
        {
            if (filter is null) return true;

            if (filter.StartCreationDate is not null && entity.CreationDate < filter.StartCreationDate)
            {
                return false;
            }

            if (filter.EndCreationDate is not null && entity.CreationDate > filter.EndCreationDate)
            {
                return false;
            }

            if (filter.StartEditDate is not null && entity.EditDate < filter.StartEditDate)
            {
                return false;
            }

            if (filter.EndEditDate is not null && entity.EditDate > filter.EndEditDate)
            {
                return false;
            }

            return true;
        }

        public async Task AddAsync(string userName, IFormFile file)
        {
            if (file is null)
            {
                throw new FileIsEmptyException();
            }

            FileDatumModel model = new FileDatumModel { UserName = userName, FileName = file.FileName, CreationDate = DateTime.Now, EditDate = DateTime.Now };

            if (await CheckExistanceInDB(userName, file.FileName))
            {
                throw new FileAlreadyExistException();
            }

            string path = GetFilePath(model);
            string directory = GetDirectoryPath(model.UserName);

            if (System.IO.File.Exists(path))
            {
                throw new FileAlreadyExistInFSException();
            }
            
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (!System.IO.File.Exists(path))
            {
                throw new UnknownException();
            }       

            await fileDataRepository.AddAsync(await GetNewEntityFromModel(model));
            await unitOfWork.SaveAsync();
        }

        public async Task<FileDatumModel> GetFileDataByIdAsync(int id)
        {
            var fileData = await fileDataRepository.GetByIdAsync(id);

            if (fileData is null)
            {
                throw new FileDoesntExistException();
            } 

            FileDatumModel model = mapper.Map<FileDatum, FileDatumModel>(fileData);

            FileInfo fileInfo = new(GetFilePath(model));

            if (fileInfo is null)
            {
                throw new FileDoesntExistInFSException();
            }
            
            model.Size = fileInfo.Length;

            return model;
        }

        public async Task<MemoryStream> GetFileByModelAsync(FileDatumModel model)
        {
            string path = GetFilePath(model);

            if (!File.Exists(path))
            {
                throw new FileDoesntExistInFSException();
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;
        }
        
        public async Task<IEnumerable<FileDatumModel>> GetAllFileModelsByUserNameAsync(string userName, FilterFileDatumModel filter)
        {
            return (await fileDataRepository.GetAllAsync()).Where(fd =>fd.UserName == userName && CheckViaFilter(fd, filter)).OrderBy(fd => fd.FileName).Select(fd => GetModelFromEntity(fd));
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await fileDataRepository.GetByIdAsync(id);

            if (File.Exists(GetFilePath(entity)))
            {
                File.Delete(GetFilePath(entity));
            }

            if (File.Exists(GetFilePath(entity)))
            {
                throw new UnknownException();
            }

            await fileDataRepository.DeleteByIdAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(FileDatumModel model)
        {
            await RemoveAsync(await GetIdByModel(model));
        }

        public async Task RenameAsync(string userName, string fileName, string newFileName)
        {
            var model = new FileDatumModel { UserName = userName, FileName = fileName, EditDate = DateTime.Now };
            var old = await GetFileDataByIdAsync(await GetIdByModel(new FileDatumModel { UserName = userName, FileName = fileName }));

            var entity =(await fileDataRepository.GetAllAsync()).SingleOrDefault(fd => fd.UserName == userName && fd.FileName == fileName);

            if (entity is null)
            {
                throw new FileDoesntExistException();
            }

            entity.FileName = newFileName;
            entity.EditDate = DateTime.Now;

            string oldPath = GetFilePath(old);
            string newPath = GetFilePath(entity);

            if (!File.Exists(oldPath))
            {
                throw new FileDoesntExistInFSException();
            }

            if (File.Exists(newPath))
            {
                throw new FileAlreadyExistInFSException();
            }

            File.Move(oldPath, newPath);

            if (!File.Exists(GetFilePath(entity)))
            {
                throw new UnknownException();
            }

            //fileDataRepository.Update(entity);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateFileAsync(FileDatumModel model, IFormFile file)
        {
            
            string path = GetFilePath(model);

            if (!File.Exists(path))
            {
                throw new FileDoesntExistInFSException();
            }
            File.Delete(path);

            if (File.Exists(path))
            {
                throw new UnknownException();
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (!File.Exists(path))
            {
                throw new UnknownException();
            }
            await unitOfWork.SaveAsync();
        }
    }
}
