import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export class Respuesta{
 constructor(
  public esExitoso:Boolean,
  public codigo:Number, 
  public uuid:String, 
  public mensaje:String, 
  public fecha:String, 
  public errores:[String],
  public resultado:[Pedido],
  ){}
}

export class Pedido{
  constructor(
   public id:string,
   public cliente:string, 
   public productos:[String], 
   public entrega:Entrega, 
   ){}
 }

 export class Entrega{
  constructor(
   public direccion:string,
   public notas:[string], 
   public telefono:String
   ){}
 }

@Injectable({
  providedIn: 'root'
})
export class PedidoService {

  constructor(private http: HttpClient) { }
  url = 'https://middlewarecomercioelectronico.azurewebsites.net/api/ConsultarPedido?code=9aobaUbDm6h5gMmDZDR29n7E1c9HJDHZT/xWWr8Ub88HhNAx77TEWQ==';
  head = new Headers({
    'Content-Type': 'application/json'
  });
  data = {
    "cliente": ""
  }
  body = JSON.stringify(this.data)
  consultarPedidos(){
    return this.http.post<Respuesta>(this.url,this.body)
  }

}
