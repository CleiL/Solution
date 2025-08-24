import { Routes } from '@angular/router';
import { authGuard } from './guard/auth.guard';


export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./pages/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./pages/register.component').then(m => m.RegisterComponent) },
  { path: 'register-medico', loadComponent: () => import('./pages/register-medico.component').then(m => m.RegisterMedicoComponent) },
  {
    path: 'home', /*canActivate: [authGuard],*/ loadComponent: () => import('./pages/layout.component').then(m => m.LayoutComponent),
    children: [
      { path: 'agenda', loadComponent: () => import('./pages/agenda.component').then(m => m.AgendaComponent) },
      { path: 'consultas', loadComponent: () => import('./pages/consultas.component').then(m => m.ConsultasComponent) }
    ]
  },
];
