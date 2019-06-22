using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    #region VÁRIAVEIS

    public InputField nick, email, senha, confirmarSenha;

    public Button[] buttons;
    public InputField[] inputFields;

    public Text iDLogado, nickLogado, emailLogado;

    private FirebaseAuth auth;
    private FirebaseUser user;

    #endregion

    void Start()
    {
        IniciarFirebase();
    }

    #region INICIANDO FIREBASE
    
    
    public void IniciarFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {

            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Verificação concluída, Firebase ativado");

                auth = FirebaseAuth.DefaultInstance;

                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
            }

            else
            {
                Debug.LogError(System.String.Format("Não foi possível resolver todas as dependências do Firebase: {0}", dependencyStatus));
            }

        });
    }

    public void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Usuário foi Desconectado");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Usuario Conectado");
            }
        }
    }

    public void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    #endregion

    #region Cadastro
    public async void LoginAnonimo()
    {
       PreCadastro();  
       await auth.SignInAnonymouslyAsync().ContinueWith(task => {


       });
       if(auth.CurrentUser != null)
       {
           SceneManager.LoadScene("Usuario Logado");  
       }
       else
       {
          PosCadastro();
          Debug.Log("Usuario nao cadastrado, tente novamente");

       }
      

    } 

    public async void CadastrarUsuario()
    {  
         
       if(senha.text == confirmarSenha.text)
       {
           PreCadastro();

           await auth.CreateUserWithEmailAndPasswordAsync(email.text, senha.text).ContinueWith(task => 
           {
                
                
           });

           if(auth.CurrentUser != null)
           {
           EnviarEmailVerificacao();

           RegistrarNick();

           PosCadastro();
   
           LimpaDados(); 
           }
           else
           { 
             Debug.Log("Verifique seu email ou sua senha!");
            
             PosCadastro();

           }

       }
       else
       {
         Debug.Log("Senhas não são iguais!");

       }

    }

    public async void EnviarEmailVerificacao()
    {

      if(auth.CurrentUser != null)
      {
        await auth.CurrentUser.SendEmailVerificationAsync().ContinueWith(task => 
        {


        });

        Debug.Log("Email enviado com sucesso!");   
       }


    }
    

    public async void RegistrarNick()
    {
        if(auth.CurrentUser != null)
        {
           UserProfile perfil = new UserProfile();

           perfil.DisplayName = nick.text;

           await auth.CurrentUser.UpdateUserProfileAsync(perfil).ContinueWith(task => 
           { 

           }); 

           Perfil();
           Logout(); 

          
        } 

    }

    public void Perfil()
    {
      
       if(auth.CurrentUser != null)
       {
          Debug.Log(auth.CurrentUser.UserId);
          Debug.Log(auth.CurrentUser.DisplayName);
          Debug.Log(auth.CurrentUser.Email); 
       }

    }

    public void Logout()
    { 
       auth.SignOut();  

    } 

    public void PreCadastro()
    { 
        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = false;
        }

        for (int i = 0; i < inputFields.Length; i++)
        {

            inputFields[i].interactable = false;
        }


    }
    public void PosCadastro()
    { 
        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = true;
        }

        for (int i = 0; i < inputFields.Length; i++)
        {

            inputFields[i].interactable = true;
        }


    }

    public void LimpaDados()
    {
      for (int i = 0; i < inputFields.Length; i++)
      {
         inputFields[i].text = "";

      }  

    }

    public void TelaLogin()
    {  
            
        SceneManager.LoadScene("Realizar Login");

    }   

    public void TeladeCadastro()
    {
     
       SceneManager.LoadScene("Criar Conta");

    } 
    
    #endregion

    public async void Login()
    {
      
      PreCadastro();

      await auth.SignInWithEmailAndPasswordAsync(email.text, senha.text).ContinueWith(task => {

      });  
      
      if(auth.CurrentUser != null)
      {
         if(auth.CurrentUser.IsEmailVerified == false)
         {
            Logout();

            PosCadastro();

            Debug.Log("Email ainda nao verificado!"); 
              
         }
         else
         {
            SceneManager.LoadScene("Usuario Logado");
              
         }
           

      }
      else
      {
        Debug.Log("Email ou senha estão incorretos!");

        PosCadastro();

      }


    }

    public async void ReenviarEmailVerificacao()
    {
      PreCadastro();
      
      await auth.SignInWithEmailAndPasswordAsync(email.text, senha.text).ContinueWith(task => 
      {

      });

      if(auth.CurrentUser != null)
      {
          
          await auth.CurrentUser.SendEmailVerificationAsync().ContinueWith(task => 
          {


          });

          Logout();  
  
          PosCadastro();

      }
      else
      {
        Debug.Log("Verifique seu email e senha!");
  
          PosCadastro();

      } 

    }

    public async void RecuperarSenha()
    {
       PreCadastro();  

       await auth.SendPasswordResetEmailAsync(email.text).ContinueWith(task =>
       {

       });

       Debug.Log("Acesse seu email para verificar e trocar sua senha!");

       PosCadastro();

    }

    public async void AtualizarNickLogado()
    {   
        if(auth.CurrentUser != null)
        {
            PreCadastro();

            UserProfile perfil = new UserProfile();

            perfil.DisplayName = nick.text;

            await auth.CurrentUser.UpdateUserProfileAsync(perfil).ContinueWith(task => 
            {

            });

            PosCadastro();

        }

    }

    public async void AtualizarSenhaLogado()
    {
        if(auth.CurrentUser != null)
        {
            PreCadastro();

            await auth.CurrentUser.UpdatePasswordAsync(senha.text).ContinueWith(task => 
            {

            });

              if(senha.text.Length > 5)
              {
                 Debug.Log("Senha atualizada com sucesso!"); 

                 
              }
              else
              {
                 Debug.Log("Certifique-se que a senha digitada contenha pelo menos 6 caracteres."); 

              }
                         
              PosCadastro();  
        }


    }

    public async void ExcluirUsuario()
    {
      if(auth.CurrentUser != null)
      {
          PreCadastro();
          await auth.CurrentUser.DeleteAsync().ContinueWith(task => 
          {

          });
           if(auth.CurrentUser == null)
           {
               SceneManager.LoadScene("Criar Conta");
           }
           else
           {
              Debug.Log("Erro, tente novamente.");

              PosCadastro();  

           }
      
        }

    }
    
    public async void ReautenticarUsuario()
    {
      Credential credential = EmailAuthProvider.GetCredential(email.text, senha.text);

      if(auth.CurrentUser != null)
      {
         PreCadastro();

         await auth.CurrentUser.ReauthenticateAsync(credential).ContinueWith(task => 
         {

         });

         PosCadastro();
         Debug.Log("Sua conta foi reautenticada!");   

      }

    }

    public async void AtualizarEmailLogado()
    {
       if(auth.CurrentUser != null)
       {
         PreCadastro();

         await auth.CurrentUser.UpdateEmailAsync(email.text).ContinueWith(task => 
         {

         });    

            if(email.text == auth.CurrentUser.Email)
            {
                Debug.Log("Email atualizado com sucesso!");        

            }
            else
            {
                Debug.Log("Erro, Email já está cadastrado");        

            } 

            PosCadastro();

       }
      }
     
      public async void VincularUsuarioAnonimo()
       { 
         if(auth.CurrentUser.IsAnonymous == true)
         {
            PreCadastro();

            Credential credential = EmailAuthProvider.GetCredential(email.text, senha.text);

            await auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => 
            {

            });

            await auth.SignInWithCredentialAsync(credential).ContinueWith(task => 
            {

            });
            if(auth.CurrentUser.IsAnonymous == false)
            {
                ReenviarEmailVerificacaoAnonimo();
                
            }
            else
            {
               Debug.Log("Verifique se o email digitado já não possui cadastro ou a senha está fora dos requisitos.");
               PosCadastro();
            }
         
          }

       }

     public void PerfilLogado()
     {
      
       if(auth.CurrentUser != null)
       {
          iDLogado.text = "ID: " + auth.CurrentUser.UserId;
          nickLogado.text = "Nick: " + auth.CurrentUser.DisplayName;
          emailLogado.text = "Email: " + auth.CurrentUser.Email;
       }

    }

    public async void ReenviarEmailVerificacaoAnonimo()
    {
      PreCadastro();
      
      await auth.SignInWithEmailAndPasswordAsync(email.text, senha.text).ContinueWith(task => 
      {

      });

      if(auth.CurrentUser != null)
      {
          
          await auth.CurrentUser.SendEmailVerificationAsync().ContinueWith(task => 
          {


          });

          Logout();  

          SceneManager.LoadScene("Realizar Login");
  
      }
      else
      {
        Debug.Log("Verifique seu email e senha!");
  
          PosCadastro();

      } 

    }


}

