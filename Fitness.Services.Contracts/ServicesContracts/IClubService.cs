﻿using Fitness.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Contracts.ServicesContracts
{
    public interface IClubService
    {
        /// <summary>
        /// Получить список всех <see cref="ClubModel"/>
        /// </summary>
        Task<IEnumerable<ClubModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="ClubModel"/> по идентификатору
        /// </summary>
        Task<ClubModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);       
    }
}
