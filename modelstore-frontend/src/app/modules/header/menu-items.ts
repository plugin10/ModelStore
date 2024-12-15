import { MenuItem } from 'primeng/api';

export const getMenuItems = (context: any): MenuItem[] => [
  {
    label: 'Strona Główna',
    command: () => {
      context.router.navigate(['/products']);
    },
  },
  {
    label: 'Produkty',
    command: () => {
      context.router.navigate(['/products']);
    },
  },
  {
    label: 'Zamówienia',
    command: () => {
      context.router.navigate(['/login']);
    },
  },
  //   {
  //     label: 'Regulamin',
  //   },
  //   {
  //     label: 'Zadania',
  //     badge: '3',
  //     command: () => {
  //       context.showTasks();
  //     },
  //   },
];
