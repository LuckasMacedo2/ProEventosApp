import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './components/eventos/eventos.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { NavComponent } from './shared/nav/nav.component';

import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EventoService } from './services/evento.service';
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';
import { ContatosComponent } from './components/contatos/contatos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PerfilComponent } from './components/user/perfil/perfil.component';
import { TituloComponent } from './shared/titulo/titulo.component';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { EventoDetalheComponent } from './components/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './components/eventos/evento-lista/evento-lista.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';

import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { LoteService } from './services/lote.service';

import { NgxCurrencyModule } from 'ngx-currency';
import { AccountService } from './services/account.service';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { HomeComponent } from './components/home/home.component';

import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { PerfilDetalheComponent } from './components/user/perfil/perfil-detalhe/perfil-detalhe.component';
import { PalestranteListaComponent } from './components/palestrantes/palestrante-lista/palestrante-lista.component';
import { PalestranteDetalheComponent } from './components/palestrantes/palestrante-detalhe/palestrante-detalhe.component';
import { RedesSociaisComponent } from './components/redesSociais/redesSociais.component';

defineLocale('pt-br', ptBrLocale);
@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    PalestrantesComponent,
    PalestranteListaComponent,
    PalestranteDetalheComponent,
    RedesSociaisComponent,
    ContatosComponent,
    DashboardComponent,
    PerfilComponent,
    PerfilDetalheComponent,
    TituloComponent,
    NavComponent,
    DateTimeFormatPipe,
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
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    TooltipModule,
    BsDropdownModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }), // neste caso usa-se o forRoot
    NgxSpinnerModule,
    NgxCurrencyModule,
    HttpClientModule
  ],
  providers: [EventoService,
              LoteService,
              AccountService,
              {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
