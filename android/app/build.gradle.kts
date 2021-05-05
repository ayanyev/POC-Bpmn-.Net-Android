import com.android.build.gradle.internal.cxx.configure.gradleLocalProperties

plugins {
    id("com.android.application")
    id("kotlin-android")
}

val composeVersion = "1.0.0-beta05"
val ktor_version = "1.5.4"
val koin_version = "3.0.1"
val kotlin_version = "1.5.0"

val properties = gradleLocalProperties(rootDir)
var processAppUrl: String? = System.getenv("PROCESS_APP_URL")
val processAppUrlHome = properties.getProperty("ip.home") ?: "NoBuildNumberFound"
val processAppUrlWork = properties.getProperty("ip.work") ?: "NoBuildNumberFound"

android {
    compileSdk = 30

    defaultConfig {
        applicationId = "com.eazzyapps.example.android"
        minSdk = 27
        targetSdk = 30
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        debug {
            isDebuggable = true
        }
        release {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android.txt"), "proguard-rules.pro")
        }
    }

    flavorDimensions.addAll(listOf("target", "location"))

    productFlavors {
        create("emulator") {
            dimension = "target"
            buildConfigField("String", "PROCESS_APP_URL", "\"http://10.0.2.2\"")
        }
        create("device") {
            dimension = "target"
        }
        create("home") {
            dimension = "location"
            buildConfigField("String", "PROCESS_APP_URL", "\"$processAppUrlHome\"")
        }
        create("work") {
            dimension = "location"
            buildConfigField("String", "PROCESS_APP_URL", "\"$processAppUrlWork\"")
        }
    }

    compileOptions {
        sourceCompatibility(JavaVersion.VERSION_1_8)
        targetCompatibility(JavaVersion.VERSION_1_8)
    }
    kotlinOptions {
        freeCompilerArgs = listOf(
            "-Xopt-in=androidx.compose.material.ExperimentalMaterialApi",
            "-Xopt-in=kotlinx.coroutines.ExperimentalCoroutinesApi",
            "-Xopt-in=kotlin.contracts.ExperimentalContracts",
            "-Xopt-in=kotlinx.coroutines.FlowPreview",
            "-Xopt-in=kotlin.Experimental",
            "-Xallow-jvm-ir-dependencies",
            "-Xskip-prerelease-check"
        )
        jvmTarget = "1.8"
    }
    buildFeatures {
        compose = true
    }
//    composeOptions {
//        kotlinCompilerVersion
//        kotlinCompilerExtensionVersion = composeVersion
//    }
}

dependencies {

    implementation(project(":scanner"))
    implementation("androidx.core:core-ktx:1.3.2")
    implementation("androidx.appcompat:appcompat:1.2.0")
    implementation("com.google.android.material:material:1.3.0")

    implementation("org.jetbrains.kotlin:kotlin-stdlib:$kotlin_version")

    implementation ("androidx.compose.compiler:compiler:$composeVersion")
    implementation("androidx.compose.ui:ui:$composeVersion")
    implementation("androidx.compose.runtime:runtime:$composeVersion")
    implementation("androidx.compose.material:material:$composeVersion")
    implementation("androidx.compose.ui:ui-tooling:$composeVersion")
    implementation("androidx.compose.material:material-icons-extended:$composeVersion")
    implementation("androidx.navigation:navigation-compose:1.0.0-alpha10")


    implementation("androidx.lifecycle:lifecycle-runtime-ktx:2.3.1")
    implementation("androidx.lifecycle:lifecycle-viewmodel-compose:1.0.0-alpha04")
    implementation("androidx.activity:activity-compose:1.3.0-alpha07")

    implementation("io.ktor:ktor-client-core:$ktor_version")
    implementation("io.ktor:ktor-client-okhttp:$ktor_version")
    implementation("io.ktor:ktor-client-websockets:$ktor_version")

    implementation("com.microsoft.signalr:signalr:6.0.0-preview.3.21201.13")
//    implementation("org.slf4j:slf4j-android:1.7.30")

    implementation("io.insert-koin:koin-android:$koin_version")
    implementation("io.insert-koin:koin-android-ext:$koin_version")
//    implementation("io.insert-koin:koin-androidx-viewmodel:2.2.2")
//    implementation("io.insert-koin:koin-androidx-scope:2.2.2")
    implementation("io.insert-koin:koin-androidx-compose:$koin_version")
    implementation("io.insert-koin:koin-test:$koin_version")

    testImplementation("junit:junit:4.13.2")
    androidTestImplementation("androidx.test.ext:junit:1.1.2")
    androidTestImplementation("androidx.test.espresso:espresso-core:3.3.0")
}