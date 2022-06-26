import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export class Evento {
   id: number;
   local: string;
   dataEvento?: Date;
   tema: string;
   qtdPessoas: number;
   lotes: Lote [];
   redesSociais: RedeSocial [];
   imagemURL: string;
   telefone: string;
   email: string;
   palestrantesEventos: Palestrante [];
}
