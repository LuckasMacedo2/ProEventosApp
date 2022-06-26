import { Evento } from "./Evento";
import { RedeSocial } from "./RedeSocial";
import { UserUpdate } from "./UserUpdate";

export class Palestrante {
  id: number ;
  miniCurriculo: string ;
  user: UserUpdate;
  redesSociais: RedeSocial [];
  palestrantesEventos: Evento [];
}
