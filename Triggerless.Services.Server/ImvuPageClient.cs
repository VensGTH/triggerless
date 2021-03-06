﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggerless.Models;

namespace Triggerless.Services.Server
{
    public class ImvuPageClient: IDisposable
    {
        private ImvuPageService _service;

        public ImvuPageClient()
        {
            _service = new ImvuPageService();
        }

        public async Task<ImvuProduct> GetHiddenProduct(long productId)
        {
            var result = new ImvuProduct { Id = productId };
            result.IsVisible = false;

            var lines = await _service.GetLines($"shop/product.php?products_id={productId}");

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 5)
                {
                    int iLeft = 14;
                    int iRight = lines[i].IndexOf(" by ", iLeft);
                    result.Name = lines[i].Substring(iLeft, iRight - iLeft);

                    iLeft = iRight + 4;
                    iRight = lines[i].IndexOf("</title>", iLeft);
                    result.CreatorName = lines[i].Substring(iLeft, iRight - iLeft);
                }

                if (i == 18)
                {
                    int iLeft = 35;
                    int iRight = lines[i].IndexOf("\"", iLeft);
                    result.ProductImage = lines[i].Substring(iLeft, iRight - iLeft);
                    break;
                }
            }

            using (var apiClient = new ImvuApiClient())
            {
                var user = await apiClient.GetUserByName(result.CreatorName);
                result.CreatorId = user.Id;
            }



            return result;

        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}