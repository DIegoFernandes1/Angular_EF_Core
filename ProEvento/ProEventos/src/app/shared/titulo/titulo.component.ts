import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  @Input() titulo: string = "";
  @Input() subTitulo: string = "Desde 2022";
  @Input() iconClass: string = "fa fa-user";
  @Input() botaoListar = false;

  constructor(private router: Router) { }


  ngOnInit() {
  }

  listar(): void{
    this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`]);
  }

}
