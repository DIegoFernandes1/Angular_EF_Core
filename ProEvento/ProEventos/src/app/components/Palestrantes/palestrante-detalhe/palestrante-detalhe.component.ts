import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {

  public form!: FormGroup;
  public situacaoForm = '';
  public corDescricao = '';

  constructor(
    private fb: FormBuilder,
    public palestranteService: PalestranteService,
    private toaster: ToastrService,
    private spineer: NgxSpinnerService
    ) { }

    ngOnInit() {
      this.validation();
      this.verificaForm();
      this.carregarPalestrante();
    }

    private validation(): void{
      this.form = this.fb.group({
        miniCurriculo: ['']
      })
    }

    private carregarPalestrante(): void {
      this.spineer.show();

      this.palestranteService.getPalestrante().subscribe(
        (palestrante: Palestrante) => {
          this.form.patchValue(palestrante)
        },
        (error) => {
          this.toaster.error('Erro ao tentar carregar o palestrante');
          console.error(error)
        }
        )
      }

      private verificaForm(): void{
        this.form.valueChanges.pipe(
          map(() => {
            this.situacaoForm = 'minicurriculo esta sendo atualizado!'
            this.corDescricao = 'text-warning'

          }),
          debounceTime(1000),
          tap(() => this.spineer.show())
          ).subscribe(
            () => {
              this.palestranteService.put({...this.form.value}).subscribe(
                () => {
                  this.situacaoForm = 'minicurriculo foi atualizado!'
                  this.corDescricao = 'text-success'

                  setTimeout(() => {
                    this.situacaoForm = 'minicurriculo foi carregado!'
                    this.corDescricao = 'text-muted'
                  }, 2000);
                },
                (error) => {
                  this.toaster.error('Erro ao tentar atualizar palestrante', 'Erro')
                  console.error(error)
                }).add(() => this.spineer.hide())
              })
            }

            public get f(): any{
              return this.form.controls;
            }

          }
