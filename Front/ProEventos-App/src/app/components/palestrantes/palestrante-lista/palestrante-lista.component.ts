import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { environment } from '@environments/environment';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {
  public Palestrantes: Palestrante[] = [];
  public eventoId = 0;
  public pagination = {} as Pagination;


  constructor(private palestranteService: PalestranteService,
              private modalService: BsModalService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService,
              private router: Router) { }

  termoBuscaChanged: Subject<string> = new Subject<string>();

  ngOnInit() {
    this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;

    this.carregarPalestrantes()
  }

  public filtrarPalestrante(evt: any)
  {
    if(this.termoBuscaChanged.observers.length === 0){

      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.palestranteService
          .getPalestrantes(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
          ).subscribe(
            (response: PaginatedResult<Palestrante[]>) => {
              this.Palestrantes = response.result;
              this.pagination = response.pagination;
              console.log(this.pagination)
            },
            (error: any) => {
              this.spinner.hide();
              this.toastr.error('Erro ao carregar os Palestrantes', 'Erro');
            },

          ).add(() => this.spinner.hide())
        }
      )
    }
    this.termoBuscaChanged.next(evt.value);
  }

  public getImagemURL(imagemName: string): string {
    if(imagemName)
      return environment.apiURL + `resources/perfil/${imagemName}`;
    else
      return `./assets/img/semImagem.jpg`;
  }

  public carregarPalestrantes(): void {
    this.spinner.show();

    this.palestranteService.getPalestrantes(this.pagination.currentPage,
                                  this.pagination.itemsPerPage).subscribe({
      next: (response: PaginatedResult<Palestrante[]>) => {
        this.Palestrantes = response.result;
        this.pagination = response.pagination;
        console.log(this.pagination)
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os Palestrantes', 'Erro');
      },
      complete: () => this.spinner.hide()
    }
    );
  }

}
