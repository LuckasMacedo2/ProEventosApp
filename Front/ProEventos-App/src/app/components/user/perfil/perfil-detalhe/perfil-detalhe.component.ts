import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PalestranteService } from '@app/services/palestrante.service';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {

  @Output() changeFormValue = new EventEmitter(); // angular/core

  userUpdate = {} as UserUpdate;
  form!: FormGroup;

  constructor(public fb: FormBuilder
    , public accountSerive: AccountService
    , private router: Router
    , private toaster: ToastrService
    , private palestranteService: PalestranteService
    , private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  private verificaForm(): void {
    this.form.valueChanges
        .subscribe(() => this.changeFormValue.emit( { ... this.form.value } ))
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountSerive.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado', 'Sucesso');
      },
      (error) => {
        console.error(error);
        this.toaster.error('Usuário carregado', 'Sucesso');
        this.router.navigate(['/dashboard'])
      }
    )
    .add(() => this.spinner.hide());
  }

  private validation(): void{
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      userName: [''],
      imagemURL: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      descricao: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', Validators.required],
      confirmePassword: ['', Validators.required],
    }, formOptions)
  }

  public resetForm(event: any): void {
    event.prevenDefault();
    this.form.reset();
  }

  onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.userUpdate = { ... this.form.value }
    this.spinner.show();

    if(this.f.funcao.value == 'Palestrante')
    {
      this.palestranteService.post().subscribe(
        () => this.toaster.success('Função palestrante ativada!', 'Sucesso'),
        (error) => {
          this.toaster.error('A função palestrante não pode ser ativada', 'Erro');
          console.error(error);
        }

      )
    }

    console.log(this.userUpdate)

    this.accountSerive.updateUSer(this.userUpdate).subscribe(
      () => this.toaster.success('Usuário atualizado', 'Sucesso'),
      (error) => {
        this.toaster.error(error.error);
        console.log(error);
      }
    )
    .add(() => this.spinner.hide());
  }

    // Método para retornar o controle
    get f(): any { return this.form.controls; }

}
