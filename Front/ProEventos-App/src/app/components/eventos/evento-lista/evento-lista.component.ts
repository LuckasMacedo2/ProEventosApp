import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject } from 'rxjs';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';


@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public modalRef?: BsModalRef;

  public eventos: Evento[] = [];
  public eventoId = 0;

  public larguraImagem: number = 150;
  public margemImagem: number = 2;
  public  exibirImagem: boolean = true;

  public pagination = {} as Pagination;

  public totalItems: number = 1;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarEventos(evt: any)
  {
    if(this.termoBuscaChanged.observers.length === 0){

      this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
        filtrarPor => {
          this.spinner.show();
          this.eventoService
          .getEventos(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
          ).subscribe(
            (response: PaginatedResult<Evento[]>) => {
              this.eventos = response.result;
              this.pagination = response.pagination;
              console.log(this.pagination)
            },
            (error: any) => {
              this.spinner.hide();
              this.toastr.error('Erro ao carregar os eventos', 'Erro');
            },

          ).add(() => this.spinner.hide())
        }
      )
    }
    this.termoBuscaChanged.next(evt.value);
  }

  public  constructor(private eventoService: EventoService,
                      private modalService: BsModalService,
                      private toastr: ToastrService,
                      private spinner: NgxSpinnerService,
                      private router: Router) { }

  public ngOnInit(): void { // Método chamado no inicio do programa
    this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;
    this.carregarEventos();
  }

  public carregarEventos(): void {
    this.spinner.show();

    this.eventoService.getEventos(this.pagination.currentPage,
                                  this.pagination.itemsPerPage).subscribe({
      next: (response: PaginatedResult<Evento[]>) => {
        this.eventos = response.result;
        this.pagination = response.pagination;
        console.log(this.pagination)
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro');
      },
      complete: () => this.spinner.hide()
    }
    );
  }

  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemURL: string): string {
    return (imagemURL !== '')
            ? `${environment.apiURL}resources/images/${imagemURL}`
            : 'assets/img/semImagem.jpg';
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number)
  {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.carregarEventos()
  }

  confirm()
  {
    this.modalRef?.hide();

    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if(result.message === 'Deletado'){
          this.toastr.success('O evento foi deletado com sucesso', 'Deletado!');
          this.carregarEventos();
        }
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());

  }

  decline(){
    this.modalRef?.hide();
  }

  detalheEvento(id: number) : void {
    this.router.navigate([`eventos/detalhe/${id}`])
  }



}
