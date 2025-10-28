<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Society_Management_System.Account.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Login - Society Management System</title>
    
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Poppins', sans-serif;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 2rem;
            position: relative;
            overflow: hidden;
        }

        /* Animated Background */
        body::before {
            content: '';
            position: absolute;
            width: 200%;
            height: 200%;
            top: -50%;
            left: -50%;
            background: radial-gradient(circle, rgba(255,255,255,0.1) 1px, transparent 1px);
            background-size: 50px 50px;
            animation: moveBackground 20s linear infinite;
            z-index: 0;
        }

        @keyframes moveBackground {
            0% { transform: translate(0, 0); }
            100% { transform: translate(50px, 50px); }
        }

        /* Floating Shapes */
        .shape {
            position: absolute;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.1);
            animation: float 8s ease-in-out infinite;
        }

        .shape1 {
            width: 200px;
            height: 200px;
            top: 10%;
            left: 10%;
            animation-delay: 0s;
        }

        .shape2 {
            width: 150px;
            height: 150px;
            top: 60%;
            right: 10%;
            animation-delay: 2s;
        }

        .shape3 {
            width: 100px;
            height: 100px;
            bottom: 20%;
            left: 20%;
            animation-delay: 4s;
        }

        @keyframes float {
            0%, 100% { transform: translateY(0px) rotate(0deg); }
            50% { transform: translateY(-30px) rotate(180deg); }
        }

        .login-container {
            position: relative;
            z-index: 1;
            width: 100%;
            max-width: 450px;
        }

        .glass-card {
            background: rgba(255, 255, 255, 0.15);
            backdrop-filter: blur(20px);
            -webkit-backdrop-filter: blur(20px);
            border-radius: 25px;
            border: 1px solid rgba(255, 255, 255, 0.3);
            padding: 3rem;
            box-shadow: 0 15px 45px rgba(0, 0, 0, 0.2);
            animation: slideUp 0.8s ease;
        }

        @keyframes slideUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .login-header {
            text-align: center;
            margin-bottom: 2.5rem;
        }

        .login-icon {
            width: 80px;
            height: 80px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 1.5rem;
            font-size: 2rem;
            color: #fff;
            animation: pulse 2s ease infinite;
        }

        @keyframes pulse {
            0%, 100% { transform: scale(1); }
            50% { transform: scale(1.05); }
        }

        .login-title {
            color: #fff;
            font-weight: 700;
            font-size: 2rem;
            margin-bottom: 0.5rem;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }

        .login-subtitle {
            color: rgba(255, 255, 255, 0.9);
            font-size: 1rem;
        }

        .form-group {
            margin-bottom: 1.5rem;
            position: relative;
        }

        .form-label {
            color: #fff;
            font-weight: 500;
            margin-bottom: 0.5rem;
            display: block;
        }

        .input-wrapper {
            position: relative;
        }

        .input-icon {
            position: absolute;
            left: 1rem;
            top: 50%;
            transform: translateY(-50%);
            color: rgba(255, 255, 255, 0.7);
            font-size: 1.1rem;
            z-index: 1;
        }

        .form-control {
            background: rgba(255, 255, 255, 0.1);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 50px;
            padding: 0.9rem 1rem 0.9rem 3rem;
            color: #fff;
            font-size: 1rem;
            transition: all 0.3s ease;
        }

        .form-control:focus {
            background: rgba(255, 255, 255, 0.15);
            border-color: rgba(255, 255, 255, 0.5);
            box-shadow: 0 0 20px rgba(255, 255, 255, 0.2);
            color: #fff;
            outline: none;
        }

        .form-control::placeholder {
            color: rgba(255, 255, 255, 0.6);
        }

        .btn-login {
            width: 100%;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            border-radius: 50px;
            padding: 1rem;
            font-size: 1.1rem;
            font-weight: 600;
            color: #fff;
            transition: all 0.3s ease;
            box-shadow: 0 5px 20px rgba(0,0,0,0.2);
            margin-top: 1rem;
        }

        .btn-login:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(0,0,0,0.3);
        }

        .btn-login:active {
            transform: translateY(0);
        }

        .error-message {
            background: rgba(220, 53, 69, 0.2);
            border: 1px solid rgba(220, 53, 69, 0.5);
            border-radius: 15px;
            padding: 1rem;
            color: #fff;
            margin-bottom: 1.5rem;
            text-align: center;
            animation: shake 0.5s ease;
        }

        @keyframes shake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-10px); }
            75% { transform: translateX(10px); }
        }

        .form-footer {
            text-align: center;
            margin-top: 2rem;
            color: rgba(255, 255, 255, 0.9);
        }

        .form-footer a {
            color: #fff;
            font-weight: 600;
            text-decoration: none;
            transition: all 0.3s ease;
        }

        .form-footer a:hover {
            text-decoration: underline;
        }

        .checkbox-wrapper {
            display: flex;
            align-items: center;
            color: rgba(255, 255, 255, 0.9);
            margin-top: 1rem;
        }

        .checkbox-wrapper input[type="checkbox"] {
            width: 18px;
            height: 18px;
            margin-right: 0.5rem;
            cursor: pointer;
        }

        .back-home {
            position: absolute;
            top: 2rem;
            left: 2rem;
            z-index: 10;
        }

        .btn-back {
            background: rgba(255, 255, 255, 0.2);
            border: 1px solid rgba(255, 255, 255, 0.3);
            border-radius: 50px;
            padding: 0.7rem 1.5rem;
            color: #fff;
            text-decoration: none;
            transition: all 0.3s ease;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }

        .btn-back:hover {
            background: rgba(255, 255, 255, 0.3);
            transform: translateX(-5px);
        }

        @media (max-width: 576px) {
            body {
                padding: 1rem;
            }

            .glass-card {
                padding: 2rem 1.5rem;
            }

            .login-title {
                font-size: 1.5rem;
            }

            .back-home {
                top: 1rem;
                left: 1rem;
            }
        }
    </style>
</head>
<body>
 <form id="form1" runat="server">
    <!-- Floating Shapes -->
    <div class="shape shape1"></div>
    <div class="shape shape2"></div>
    <div class="shape shape3"></div>

    <!-- Back to Home Button -->
    <div class="back-home">
        <asp:HyperLink ID="lnkBackHome" runat="server" NavigateUrl="~/Default.aspx" CssClass="btn-back">
            <i class="fas fa-arrow-left"></i> Back to Home
        </asp:HyperLink>
    </div>

    <!-- Login Container -->
    <div class="login-container">
        <div class="glass-card">
            <!-- Header -->
            <div class="login-header">
                <div class="login-icon">
                    <i class="fas fa-user-lock"></i>
                </div>
                <h1 class="login-title">Welcome Back</h1>
                <p class="login-subtitle">Sign in to access your dashboard</p>
            </div>

            <!-- Error Message -->
            <asp:Panel ID="pnlInfo" runat="server" CssClass="alert alert-success" Visible="false">
    <asp:Label ID="lblInfo" runat="server" />
</asp:Panel>


            <asp:Panel ID="pnlError" runat="server" Visible="false" CssClass="error-message">
                <i class="fas fa-exclamation-circle"></i>
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Username -->
            <div class="form-group">
                <label class="form-label">Username</label>
                <div class="input-wrapper">
                    <i class="fas fa-user input-icon"></i>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                                 placeholder="Enter your username" AutoComplete="off" />
                </div>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                                            ControlToValidate="txtUsername"
                                            ErrorMessage="Username is required"
                                            ForeColor="White"
                                            Display="Dynamic"
                                            CssClass="mt-2 d-block" />
            </div>

            <!-- Password -->
            <div class="form-group">
                <label class="form-label">Password</label>
                <div class="input-wrapper">
                    <i class="fas fa-lock input-icon"></i>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                 CssClass="form-control" placeholder="Enter your password" />
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                            ControlToValidate="txtPassword"
                                            ErrorMessage="Password is required"
                                            ForeColor="White"
                                            Display="Dynamic"
                                            CssClass="mt-2 d-block" />
            </div>

            <!-- Remember Me -->
            <div class="checkbox-wrapper">
                <asp:CheckBox ID="chkRememberMe" runat="server" />
                <label for="<%= chkRememberMe.ClientID %>">Remember me</label>
            </div>

            <!-- Login Button -->
            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-login"
                        Text="Sign In" OnClick="BtnLogin_Click" />

            <!-- Footer -->
            <div class="form-footer">
                Don't have an account?
                <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Account/Register.aspx">
                    Register here
                </asp:HyperLink>
            </div>
        </div>
    </div>
</form>

<!-- Bootstrap JS -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

<script>
    // Floating animation on input focus
    document.querySelectorAll('.form-control').forEach(input => {
        input.addEventListener('focus', function () {
            this.parentElement.style.transform = 'translateY(-2px)';
            this.parentElement.style.transition = 'all 0.3s ease';
        });

        input.addEventListener('blur', function () {
            this.parentElement.style.transform = 'translateY(0)';
        });
    });
</script>
</body>
</html>