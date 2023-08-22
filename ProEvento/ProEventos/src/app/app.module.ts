import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './components/Eventos/Eventos.component';
import { EventoDetalheComponent } from './components/Eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/Eventos/evento-lista/evento-lista.component';
import { PalestrantesComponent } from './components/Palestrantes/Palestrantes.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { TituloComponent } from './shared/titulo/titulo.component';
import { NavComponent } from './shared/nav/nav.component';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import { NgxCurrencyModule } from "ngx-currency";

import { EventoService } from './services/evento.service';
import { LoteService } from './services/Lote.service';
import { AccountService } from './services/account.service';

import { JwtInterceptor } from './interceptors/jwt.interceptor';

import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { HomeComponent } from './components/home/home.component';



defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AppComponent,
      EventosComponent,
      PalestrantesComponent,
      ContatosComponent,
      DashboardComponent,
      PerfilComponent,
      NavComponent,
      DateTimeFormatPipe,
      TituloComponent,
      EventoDetalheComponent,
      EventoListaComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      HomeComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    TooltipModule.forRoot(),
    NgxCurrencyModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({
        timeOut: 5000,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
        progressBar: true,
      }),
    NgxSpinnerModule,
    BsDatepickerModule.forRoot(),
  ],
  providers: [
    EventoService,
    LoteService,
    AccountService,
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
