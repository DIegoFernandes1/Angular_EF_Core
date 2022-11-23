import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventoDetalheComponent } from './components/Eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/Eventos/evento-lista/evento-lista.component';
import { EventosComponent } from './components/Eventos/Eventos.component';
import { PalestrantesComponent } from './components/Palestrantes/Palestrantes.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user.component';

const routes: Routes = [
  {
    path: 'user', component: UserComponent,
    children:[
      {path: 'login', component: LoginComponent},
      {path: 'registration', component: RegistrationComponent},
    ]
  },
  {path: 'user/perfil', component: PerfilComponent},
  {path: 'eventos', redirectTo: 'eventos/lista'},
  {path: 'dashboard', component: DashboardComponent},
  {path: 'eventos', component: EventosComponent,
  children:[
    {path: 'detalhe/:id', component: EventoDetalheComponent},
    {path: 'detalhe', component: EventoDetalheComponent},
    {path: 'lista', component: EventoListaComponent},
  ]
},
{path: 'palestrantes', component: PalestrantesComponent},
{path: 'contatos', component: ContatosComponent},
{path: '', redirectTo: 'dashboard', pathMatch: 'full'},
{path: '**', redirectTo: 'dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
