import { Component, OnInit } from '@angular/core';
import { PedidoService, Respuesta } from '../service/data/pedido.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  sinDatos = false
  pedidos = []
  constructor(private service: PedidoService) { }

  ngOnInit(): void {
        //console.log(this.service.executeDeliveryService());
        this.service.consultarPedidos().subscribe(
          response => {
            if (response.esExitoso) {
              this.pedidos= response.resultado
            }else{
              this.sinDatos = true
            }
          }
        );
  }
}
