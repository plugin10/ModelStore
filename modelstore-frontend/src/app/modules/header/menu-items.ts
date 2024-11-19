import { MenuItem } from 'primeng/api';

export const getMenuItems = (context: any): MenuItem[] => [
  {
    label: 'Produkty',
    command: () => {
      context.router.navigate(['/products']);
    },
  },
  {
    label: 'Koszyk',
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
