﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModelStore.Application.Models
{
    public partial class Product
    {
        public Guid Id { get; init; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Slug => GenetateSlug();

        public int Ammount { get; set; }

        public List<string> Categories { get; init; } = new();

        public string Description { get; set; }

        private string GenetateSlug()
        {
            var sluggedName = SlugRegex().Replace(Name, string.Empty)
                .ToLower().Replace(" ", "-");

            var sluggedBrand = Regex.Replace(Brand, "[^0-9A-Za-z _-]", string.Empty)
                .ToLower().Replace(" ", "-");

            return $"{sluggedName}-{sluggedBrand}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}