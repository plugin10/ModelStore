import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CategoryMapperService {
  public categories = [
    {
      code: 1,
      name: 'Figurki',
    },
    {
      code: 2,
      name: 'Farba',
    },
    {
      code: 3,
      name: 'Narzędzie',
    },
  ];

  mapIdToCategory(id: number) {
    return this.categories.find((category) => category.code === id) || null;
  }

  mapCategoryToId(categoryName: string) {
    const category = this.categories.find(
      (category) => category.name === categoryName
    );
    return category ? category.code : null;
  }
}
